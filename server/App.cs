using Server.Models;

namespace Server;

public static class App
{
    public static void Setup(this IConfiguration config)
    {
        ImageServerHost = config[nameof(ImageServerHost)];

        CtfileToken = config[nameof(CtfileToken)];
        CtDirectBaseUrl = config[nameof(CtDirectBaseUrl)].TrimEnd('/')+'/';

        ClientApps = config.GetSection(nameof(ClientApps)).Get<ClientApp[]>();
    }

    public static string ImageServerHost { get; private set; }

    public static string CtfileToken { get; private set; }
    public static string CtDirectBaseUrl { get; private set; }

    public static ClientApp[] ClientApps { get; private set; }
}
