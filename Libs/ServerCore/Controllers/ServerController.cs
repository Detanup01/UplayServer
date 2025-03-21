using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using NetCoreServer;
using ServerCore.HTTP;
using ServerCore.Models;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Authentication;

namespace ServerCore.Controllers;

public static class ServerController
{
    static UplayServer? server;
    static Dictionary<string, Dictionary<HTTPAttribute, MethodInfo>> HTTP_Plugins = [];
    static Dictionary<string, Dictionary<string, MethodInfo>> WS_Plugins = [];
    static Dictionary<HTTPAttribute, MethodInfo> Main_HTTP = [];
    static Dictionary<string, MethodInfo> Main_WS = [];
    public static void Start()
    {
        Directory.CreateDirectory("logs");
        DebugPrinter.EnableLogs = true;
        //DebugPrinter.PrintToConsole = true;
        var ServerManagerAssembly = Assembly.GetAssembly(typeof(ServerController));
        ArgumentNullException.ThrowIfNull(ServerManagerAssembly, nameof(ServerManagerAssembly));
        SslContext? context = CertHelper.GetContextNoValidate(SslProtocols.Tls12, $"cert/services.pfx", ServerConfig.Instance.CERT.ServicesCertPassword);
        server = new UplayServer(context, IPAddress.Any, 443);
        Main_HTTP = AttributeMethodHelper.UrlHTTPLoader(ServerManagerAssembly);
        Main_WS = AttributeMethodHelper.UrlWSLoader(ServerManagerAssembly);
        AddRoutes(ServerManagerAssembly);
        server.DoReturn404IfFail = false;
        server.ReceivedFailed += Failed;
        server.OnSocketError += OnSocketError;
        server.ReceivedRequestError += RecvReqError;
        server.Context.ClientCertificateRequired = false;
        server.Start();
        Console.WriteLine("Server started on " + server.Address);
    }
    public static UplayServer? GetServer()
    {
        return server;
    }

    private static void RecvReqError(object? sender, (HttpRequest request, string error) e)
    {
        Console.WriteLine("RecvReqError " + e.request.Url + " " + e.error);
    }

    private static void OnSocketError(object? sender, SocketError e)
    {
        Console.WriteLine("OnSocketError " + e);
    }

    public static void Failed(object? sender, HttpRequest request)
    {
        File.AppendAllText("REQUESTED.txt", request.Method + " " + request.Url + "\n" + request.Body + "\n" + request.ToString());
    }


    public static void Stop()
    {
        if (server != null)
        {
            server.Stop();
            server = null;
        }

        Console.WriteLine("Server stopped.");
    }

    public static void AddRoutes(Assembly assembly)
    {
        if (server == null)
            return;
        var name = assembly.GetName().FullName;
        HTTP_Plugins.Add(name, AttributeMethodHelper.UrlHTTPLoader(assembly));
        WS_Plugins.Add(name, AttributeMethodHelper.UrlWSLoader(assembly));
        server.MergeWSAttribute(assembly);
        server.MergeAttribute(assembly);
    }

    public static void RemoveRoutes(Assembly assembly)
    {
        var name = assembly.GetName().FullName;
        HTTP_Plugins.Remove(name);
        WS_Plugins.Remove(name);
        if (server == null)
            return;
        server.HTTP_AttributeToMethods = Main_HTTP;
        server.WS_AttributeToMethods = Main_WS;
        foreach (var plugin in HTTP_Plugins)
        {
            if (plugin.Key == name)
                continue;

            foreach (var item in plugin.Value)
            {
                server.HTTP_AttributeToMethods.TryAdd(item.Key, item.Value);
            }
        }
        foreach (var plugin in WS_Plugins)
        {
            if (plugin.Key == name)
                continue;

            foreach (var item in plugin.Value)
            {
                server.WS_AttributeToMethods.TryAdd(item.Key, item.Value);
            }
        }
    }
}
