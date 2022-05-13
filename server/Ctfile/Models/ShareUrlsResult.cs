namespace Server.Ctfile.Models;

public class ShareUrlsResult : CtResult
{
    public Item[] results { get; set; }

    public class Item
    {
        public string key { get; set; }

        public string name { get; set; }

        public string weblink { get; set; }
        /// <summary>
        /// 直连分享链接（文档中字段名，实测 drlink）
        /// </summary>
        public string directlink { get; set; }

        /// <summary>
        /// 直连分享链接（实测字段名）
        /// </summary>
        public string drlink { get; set; }

        public string size { get; set; }

        public string date { get; set; }
    }
}