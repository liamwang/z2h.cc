using Server.Utils;

namespace Server.Ctfile;

public static class UrlUtil
{
    // 保留标识，约定以“*”作为分隔符
    // 将来可用作URL转换算法的版本号、存储机器号等
    const string _flag = "0*";

    /// <summary>
    /// 城通网盘直连URL转为图片对外代理URL
    /// 直连URL形如：https://drfs.ctcontents.com/file/12345678/578836030/882cc2/images/liamw/578836030.jpg
    /// 转换后URL形如：https://z2h.cc/<AppId>/<Flag>*<Hash>
    /// </summary>
    public static string BuildProxyUrl(string baseUrl, string storageUrl)
    {
        // 去除 BaseUrl 后的 Url 分隔成 5 段（第 5 段与第 1 段相同）
        // 形如：578836030/882cc2/images/liamw/578836030.jpg
        var pieces = storageUrl.Replace(App.CtDirectBaseUrl, string.Empty).Split('/');

        // 第 1 段为数字，对应文件ID
        var fileIdHash = Radix.Encode(long.Parse(pieces[0]), 16);
        fileIdHash = StrUtil.Mixup(fileIdHash); // 打乱

        // 第 2 段为十六进制（应该是随机码，保留原样）
        var randomHash = pieces[1];

        // 第 3 段固“images”对应公共云盘的图片存储跟目录，固定值

        // 第 4 段为应用ID对应的文件夹名
        var appId = pieces[3];

        // 第 5 段为文件名
        var name = pieces[4];
        var ext = name[name.IndexOf('.')..];

        // 缩短后文件 Url
        return $"{baseUrl}/{appId}/{_flag}{fileIdHash}-{randomHash}{ext}";
    }

    /// <summary>
    /// <see cref="BuildProxyUrl"/>的反向操作
    /// </summary>
    public static string ResolveStorageUrl(string appId, string slug)
    {
        slug = slug.Replace(_flag, string.Empty);
        try
        {
            var ext = slug[slug.IndexOf('.')..];
            var str = slug.Replace(ext, string.Empty);
            var splits = str.Split('-');

            var fileIdHash = StrUtil.UnMixup(splits[0]);
            var fileId = Radix.Decode(fileIdHash, 16);

            var random = splits[1];

            return $"{App.CtDirectBaseUrl}{fileId}/{random}/images/{appId}/{fileId}{ext}";
        }
        catch
        {
            return null;
        }
    }
}
