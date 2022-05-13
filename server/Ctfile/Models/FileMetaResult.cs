namespace Server.Ctfile.Models;

public class FileMetaResult : CtResult
{
    public long file_id { get; set; }
    public string name { get; set; }
    public string extension { get; set; }
    public long date { get; set; }
    public int size { get; set; }
    public int views { get; set; }
}
