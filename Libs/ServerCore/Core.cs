﻿using ServerCore.DemuxResponders;
using ServerCore.Extra;
using ServerCore.DB;
using ServerCore.Models;
using ServerCore.HTTP;
using ServerCore.Controllers;

namespace ServerCore;

public class CoreRun
{
    public static void Start()
    {
        Prepare.MakeAll();
        JWTController.CreateRSA();
        PluginHandle.LoadPlugins();
        ServerController.Start();
        Console.WriteLine(ServerConfig.Instance.DemuxUrl);
        Console.WriteLine(ServerConfig.Instance.HTTPS_Url);
        //DemuxServer.Start();

    }

    public static void Stop()
    {
        PluginHandle.UnloadPlugins();
        //DemuxServer.Stop();
        ServerController.Stop();
    }
}
