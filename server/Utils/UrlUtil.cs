using ImageServer.Utils;

namespace ImageServer.Ctfile;

public static class UrlUtil
{
    // 保留标识
    // 将来可用作URL转换算法的版本号及存储机器号等
    const ushort FLAG = 0;
    // URL 字符串压缩进制
    const int RADIS = 62;

    /// <summary>
    /// 城通网盘直连URL转为图片对外代理URL
    /// 直连URL形如：https://drfs.ctcontents.com/file/12345678/578836030/882cc2/images/liamw/1652279689.jpg
    /// 转换后URL形如：https://z2h.cc/<AppId>/<Flag>*<Hash>
    /// </summary>
    public static string BuildProxyUrl(string baseUrl, string storageUrl)
    {
        // 去除 BaseUrl 后的 Url 分隔成 5 部分
        // 形如：578836030/882cc2/images/liamw/1652279689.jpg
        var pieces = storageUrl.Replace(App.CtDirectBaseUrl, string.Empty).Split('/');

        // 第 0 段为数字，对应文件ID
        var fileIdHash = Radix.Encode(long.Parse(pieces[0]), RADIS);

        // 第 1 段为十六进制（应该是随机码，不确定）
        var randomHash = Radix.Encode(Radix.Decode(pieces[1], 16), RADIS);

        // 第 2 段固“images”对应公共云盘的图片存储跟目录，固定值

        // 第 3 段为应用ID对应的文件夹名
        var appId = pieces[3];

        // 第 4 段为时间戳文件名
        var name = pieces[4];
        var ext = name[name.IndexOf('.')..];
        var time = long.Parse(name.Replace(ext, string.Empty));
        var timeHash = Radix.Encode(time, RADIS);

        // 对组成的Hash进行混淆
        var raw = $"{fileIdHash}-{randomHash}-{timeHash}";
        var mixup = StrUtil.Mixup(raw);

        // 缩短后文件 Url
        return $"{baseUrl}/{appId}/{FLAG}*{mixup}{ext}";
    }

    /// <summary>
    /// <see cref="BuildProxyUrl"/>的反向操作
    /// </summary>
    public static string ResolveStorageUrl(string appId, string slug)
    {
        slug = slug.Replace($"{FLAG}*", string.Empty);
        try
        {
            var ext = slug[slug.IndexOf('.')..];
            var mixup = slug.Replace(ext, string.Empty);

            var raw = StrUtil.UnMixup(mixup);

            var splits = raw.Split('-');

            var fileId = Radix.Decode(splits[0], RADIS);
            var random = Radix.Encode(Radix.Decode(splits[1], RADIS), 16);
            var time = Radix.Decode(splits[2], RADIS);

            return $"{App.CtDirectBaseUrl}{fileId}/{random}/images/{appId}/{time}{ext}";
        }
        catch
        {
            return null;
        }
    }
}
