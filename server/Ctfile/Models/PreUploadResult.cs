namespace Server.Ctfile.Models;

public class PreUploadResult : CtResult
{
    public int exists { get; set; }
    public string upload_url { get; set; }
}
