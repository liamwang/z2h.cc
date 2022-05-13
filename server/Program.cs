using Server;
using Server.Ctfile;
using Server.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Setup();

builder.Services.AddHttpClient<CtHttp>();
builder.Services.AddMemoryCache();

var app = builder.Build();

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

    var baseUrl = $"{request.Scheme}://{request.Host}";
    var publicUrl = UrlUtil.BuildProxyUrl(baseUrl, storageUrl);

    return Results.Ok(new UploadResult { Url = publicUrl });
});


app.Map(@"{appId}/{slug}",
    async (string appId, string slug, HttpContext context, HttpClient http) =>
{
    var storageUrl = UrlUtil.ResolveStorageUrl(appId, slug);
    if (storageUrl is null)
    {
        context.Response.StatusCode = 404;
        return;
    }

    using var request = new HttpRequestMessage(HttpMethod.Get, storageUrl);
    using var response = await http.SendAsync(request);

    if (response.StatusCode != System.Net.HttpStatusCode.OK)
    {
        context.Response.StatusCode = (int)response.StatusCode;
    }

    await response.Content.CopyToAsync(context.Response.Body);
});

app.Run();