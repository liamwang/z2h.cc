using System.Text.RegularExpressions;
using Server;
using Server.Ctfile;
using Server.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Setup();

builder.Services.AddHttpClient<CtHttp>();
builder.Services.AddMemoryCache();

var app = builder.Build();

var isProd = app.Environment.IsProduction();

// 由上层代理做跳转
// app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapPost("/upload", async (HttpRequest request, CtHttp ct) =>
{
    var form = await request.ReadFormAsync();

    var appId = form["appId"];
    var appToken = form["appToken"];

    var clientApp = App.ClientApps
        .FirstOrDefault(x => x.Id == appId && x.Token == appToken);

    Error.ThrowBadRequestIf(clientApp is null, "身份验证失败");

    var file = form.Files["file"];

    Error.ThrowBadRequestIf(file is null || file.Length == 0, "文件未上传");

    await using var stream = file.OpenReadStream();

    var storageUrl = await ct.UploadAsync(stream, file.FileName, clientApp.DirectoryId);

    // 上层代理不转发 Scheme，生产环境强制使用 Https
    var baseUrl = isProd ? $"https://{App.ImageServerHost}" : $"{request.Scheme}://{request.Host}";
    var publicUrl = UrlUtil.BuildPublicUrl(baseUrl, storageUrl);

    return Results.Ok(new UploadResult { Url = publicUrl });
});

app.MapGet(@$"{{appId}}/{{slug:regex(^{Regex.Escape(UrlUtil.FLAG)})}}",
    async (string appId, string slug, HttpContext context, HttpClient http) =>
{
    var storageUrl = UrlUtil.ResolveStorageUrl(appId, slug);
    Error.ThrowNotFoundIf(storageUrl == null);

    using var request = new HttpRequestMessage(HttpMethod.Get, storageUrl);
    using var response = await http.SendAsync(request);

    if (response.StatusCode != System.Net.HttpStatusCode.OK)
    {
        context.Response.StatusCode = (int)response.StatusCode;
    }

    await response.Content.CopyToAsync(context.Response.Body);
})
.RequireHost(isProd ? App.ImageServerHost : null);

app.Run();
