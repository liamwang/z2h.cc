namespace Server.Ctfile.Models;

public class RenameFileParam : CtParam
{
    public string file_id { get; set; }
    public string name { get; set; }
    public byte is_rename { get => 1; }
}
