using System.Security.Cryptography;
using System.Text.Json;
using ImageServer.Ctfile.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ImageServer.Ctfile;

public class CtHttp
{
    private readonly HttpClient _http;
    private readonly ILogger _logger;
    private readonly IMemoryCache _cache;

    public CtHttp(HttpClient http, ILogger<HttpClient> logger, IMemoryCache cache)
    {
        _http = http;
        _logger = logger;
        _cache = cache;
        _http.BaseAddress = new Uri("https://rest.ctfile.com/v1/public/");
    }

    public async Task<TResult> PostAsync<TParam, TResult>(string path, TParam param, Func<TResult, bool> errorPredicate = null)
        where TParam : CtParam
        where TResult : CtResult
    {
        param.SetSesstion(App.CtfileToken);

        using var response = await _http.PostAsJsonAsync(path, param);

        // var text = await response.Content.ReadAsStringAsync();
        var result = await response.Content.ReadFromJsonAsync<TResult>();

        if (result.code != 200 || (errorPredicate != null && errorPredicate(result)))
        {
            _logger.LogError("城通接口异常：{} {}", result.code, result.message);
        }

        return result;
    }

    public Task<string> GetUploadUrlAsync(string folderId)
    {
        return _cache.GetOrCreateAsync($"upload_{folderId}", async (cache) =>
        {
            var result = await PostAsync<PreUploadParam, PreUploadResult>(
                "file/upload",
                new PreUploadParam { folder_id = folderId },
                r => r.upload_url?.Length == 0);

            // 从 URL 中读取上传连接过期时间（24 小时）
            //var expires = long.Parse(HttpUtility.ParseQueryString(result.upload_url).Get("ctt"));
            var expires = 24 * 60 * 60;
            cache.AbsoluteExpiration = DateTimeOffset.FromUnixTimeSeconds(expires - 600);

            return result.upload_url;
        });
    }

    public async Task<ShareUrlsResult.Item> GetShareUrlsAsync(long fileId)
    {
        var result = await PostAsync<ShareUrlsParam, ShareUrlsResult>(
            "file/share",
            new ShareUrlsParam { ids = new[] { "f" + fileId } });
        return result.results[0];
    }

    public async Task<string> GetDirectUrlAsync(long fileId)
    {
        var result = await GetShareUrlsAsync(fileId);
        return result.drlink ?? result.directlink;
    }

    /// <summary>
    /// 上传文件并返回直连下载链接
    ///  https://openapi.ctfile.com/docs/ctfile-open-api/ctfile-open-api-1c9m4d9fhfs5j
    /// </summary>
    public async Task<string> UploadAsync(Stream stream, string fileName, long dirId)
    {
        var name = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + fileName[fileName.IndexOf('.')..];

        // var md5Bytes = MD5.Create().ComputeHash(stream);
        // var md5Str = BitConverter.ToString(md5Bytes).Replace("-", string.Empty).ToLower();
        // var pre = await PostAsync<PreUploadParam, PreUploadResult>("file/upload", new PreUploadParam
        // {
        //     folder_id = "d" + dirId,
        //     checksum = md5Str,
        //     //size = fs.Length.ToString(),
        //     //name = name,
        // });

        var uploadUrl = await GetUploadUrlAsync("d" + dirId);

        using var content = new MultipartFormDataContent
        {
            { new StringContent(name), "name" },
            { new StringContent(stream.Length.ToString()), "filesize" },
            { new StreamContent(stream), "file", name }
        };

        using var res = await _http.PostAsync(uploadUrl, content);

        var resText = await res.Content.ReadAsStringAsync();

        if (long.TryParse(resText, out long fileId))
        {
            return await GetDirectUrlAsync(fileId);
        }
        else if (resText.Contains('{'))
        {
            var result = JsonSerializer.Deserialize<CtResult>(resText);
            _logger.LogError("城通上传文件异常：{} {}", result.code, result.message);
        }

        _logger.LogError("城通上传文件异常：{}", resText);

        return null;
    }
}
