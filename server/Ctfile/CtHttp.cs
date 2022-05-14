using System.Text.Json;
using Server.Ctfile.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Server.Ctfile;

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

        var text = await response.Content.ReadAsStringAsync();
        var result = await response.Content.ReadFromJsonAsync<TResult>();

        if (result.code != 200 || (errorPredicate != null && errorPredicate(result)))
        {
            _logger.LogError("城通接口异常：{} {}", result.code, result.message);
            Error.Throw("存储服务接口异常", ErrorCode.ServerError);
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

    public Task<FileMetaResult> GetFileMetaAsync(long fileId)
    {
        return PostAsync<FileMetaParam, FileMetaResult>(
            "file/meta",
            new FileMetaParam { file_id = "f" + fileId });
    }

    public Task<CtResult> RenameFileAsync(long fileId, string fileName)
    {
        return PostAsync<RenameFileParam, CtResult>(
            "file/modify_meta",
            new RenameFileParam { file_id = "f" + fileId, name = fileName });
    }

    /// <summary>
    /// 上传文件并返回直连下载链接
    ///  https://openapi.ctfile.com/docs/ctfile-open-api/ctfile-open-api-1c9m4d9fhfs5j
    /// </summary>
    public async Task<string> UploadAsync(Stream stream, string fileName, long dirId)
    {
        var ext = UrlUtil.GetFileExt(fileName);
        var name = Guid.NewGuid().ToString() + ext;

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
        var idText = await res.Content.ReadAsStringAsync();

        if (long.TryParse(idText, out long fileId))
        {
            // 为了简化URL，将文件名重命名为文件ID
            await RenameFileAsync(fileId, fileId + ext);
            return await GetDirectUrlAsync(fileId);
        }
        else if (idText.Contains('{'))
        {
            var result = JsonSerializer.Deserialize<CtResult>(idText);
            _logger.LogError("城通上传文件异常：{} {}", result.code, result.message);
        }
        else
        {
            _logger.LogError("城通上传文件异常：{}", idText);
        }

        throw new Error("上传文件发生异常", ErrorCode.ServerError);
    }
}
