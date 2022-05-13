namespace Server.Ctfile.Models;

public class PreUploadParam : CtParam
{
    public string folder_id { get; set; }
    public string checksum { get; set; }
    public string name { get; set; }
    public string size { get; set; }
}
