namespace Server.Ctfile;

public static class UrlUtil
{
    public static string GetFileExt(string file)=> file[file.LastIndexOf('.')..];


    // 保留标识，约定以“*”作为分隔符
    // 将来可用作URL转换算法的版本号、存储机器号等
    public const string FLAG = "0*";

    /// <summary>
    /// 城通网盘直连URL转为图片对外代理URL
    /// </summary>
    public static string BuildPublicUrl(string baseUrl, string storageUrl)
    {
        // 城通直连 URL 去除 BaseUrl 后，分隔成 5 段（第 5 段与第 1 段相同）
        // 形如：578836030/882cc2/images/liamw/578836030.jpg
        var pieces = storageUrl.Replace(App.CtDirectBaseUrl, string.Empty).Split('/');

        // 第 1 段为数字，对应文件ID
        var fileId = pieces[0];

        // 第 2 段为十六进制字符串
        var random = pieces[1];

        // 第 3 段固“images”对应公共云盘的图片存储跟目录，固定值

        // 第 4 段为应用ID对应的文件夹名
        var appId = pieces[3];

        // 第 5 段为文件名
        var ext = GetFileExt(pieces[4]);

        // 缩短后文件 Url
        return $"{baseUrl}/{appId}/{FLAG}{fileId}-{random}{ext}";
    }

    /// <summary>
    /// <see cref="BuildPublicUrl"/>的反向操作
    /// </summary>
    public static string ResolveStorageUrl(string appId, string slug)
    {
        slug = slug.Replace(FLAG, string.Empty);
        try
        {
            var ext = GetFileExt(slug);
            var str = slug.Replace(ext, string.Empty);
            var splits = str.Split('-');

            var fileId = splits[0];
            var random = splits[1];

            return $"{App.CtDirectBaseUrl}{fileId}/{random}/images/{appId}/{fileId}{ext}";
        }
        catch
        {
            return null;
        }
    }
}
