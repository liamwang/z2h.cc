namespace ImageServer.Ctfile.Models;

public class CtParam
{
    public string session { get; private set; }

    public void SetSesstion(string token) => session = token;
}
