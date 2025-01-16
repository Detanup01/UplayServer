using ModdableWebServer.Attributes;
using ModdableWebServer.Helper;
using ModdableWebServer.Servers;
using NetCoreServer;
using ServerCore.Models;
using System.Net;
using System.Reflection;

namespace ServerCore.HTTP;

public class ServerManager
{
    static WSS_Server? WSS_Server = null;
    static Dictionary<string, Dictionary<HTTPAttribute, MethodInfo>> HTTP_Plugins = [];
    static Dictionary<string, Dictionary<string, MethodInfo>> WS_Plugins = [];
    static Dictionary<HTTPAttribute, MethodInfo> Main_HTTP = [];
    static Dictionary<string, MethodInfo> Main_WS = [];

    public static void Start()
    {
        DebugPrinter.EnableLogs = true;
        //DebugPrinter.PrintToConsole = true;
        var ServerManagerAssembly = Assembly.GetAssembly(typeof(ServerManager));
        ArgumentNullException.ThrowIfNull(ServerManagerAssembly, nameof(ServerManagerAssembly));
        SslContext? context = null;

        context = CertHelper.GetContextNoValidate(System.Security.Authentication.SslProtocols.Tls12, $"cert/services.pfx", ServerConfig.Instance.CERT.ServicesCertPassword);
        WSS_Server = new(context, IPAddress.Parse(ServerConfig.Instance.HTTPS_Ip), ServerConfig.Instance.HTTPS_Port);
        Main_HTTP = AttributeMethodHelper.UrlHTTPLoader(ServerManagerAssembly);
        Main_WS = AttributeMethodHelper.UrlWSLoader(ServerManagerAssembly);
        AddRoutes(ServerManagerAssembly);
        WSS_Server.DoReturn404IfFail = false;
        WSS_Server.ReceivedFailed += Failed;
        WSS_Server.Start();
        Console.WriteLine("Server started on " + WSS_Server.Address);

    }

    public static void Failed(object? sender, HttpRequest request)
    {
        File.AppendAllText("REQUESTED.txt", request.Method + " " + request.Url + "\n" + request.Body + "\n" + request.ToString());
    }


    public static void Stop()
    {
        if (WSS_Server != null)
        {
            WSS_Server.Stop();
            WSS_Server = null;
        }

        Console.WriteLine("Server stopped.");
    }

    public static void AddRoutes(Assembly assembly)
    {
        if (WSS_Server != null)
        {
            var name = assembly.GetName().FullName;
            HTTP_Plugins.Add(name, AttributeMethodHelper.UrlHTTPLoader(assembly));
            WS_Plugins.Add(name, AttributeMethodHelper.UrlWSLoader(assembly));
            WSS_Server.MergeWSAttribute(assembly);
            WSS_Server.MergeAttribute(assembly);
        }
    }

    public static void RemoveRoutes(Assembly assembly)
    {
        var name = assembly.GetName().FullName;
        HTTP_Plugins.Remove(name);
        WS_Plugins.Remove(name);
        if (WSS_Server == null)
            return;
        WSS_Server.HTTP_AttributeToMethods = Main_HTTP;
        WSS_Server.WS_AttributeToMethods = Main_WS;
        foreach (var plugin in HTTP_Plugins)
        {
            if (plugin.Key == name)
                continue;

            foreach (var item in plugin.Value)
            {
                WSS_Server.HTTP_AttributeToMethods.TryAdd(item.Key, item.Value);
            }
        }
        foreach (var plugin in WS_Plugins)
        {
            if (plugin.Key == name)
                continue;

            foreach (var item in plugin.Value)
            {
                WSS_Server.WS_AttributeToMethods.TryAdd(item.Key, item.Value);
            }
        }
    }
}
