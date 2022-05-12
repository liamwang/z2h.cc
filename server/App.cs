﻿using ImageServer.Models;

namespace ImageServer;

public static class App
{
    public static void Setup(this IConfiguration config)
    {
        CtfileToken = config[nameof(CtfileToken)];
        CtDirectBaseUrl = config[nameof(CtDirectBaseUrl)].TrimEnd('/')+'/';

        ClientApps = config.GetSection(nameof(ClientApps)).Get<ClientApp[]>();
    }

    public static string CtfileToken { get; private set; }
    public static string CtDirectBaseUrl { get; private set; }

    public static ClientApp[] ClientApps { get; private set; }
}