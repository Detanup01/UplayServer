using ServerCore.Extra.Interfaces;
using SharedLib.Shared;
using System.Reflection;

namespace ServerCore.Extra;

public class PluginHandle
{
    private static Dictionary<string, IPlugin> pluginsList = [];
    public static void LoadPlugins()
    {
        string currdir = Directory.GetCurrentDirectory();

        if (!Directory.Exists(Path.Combine(currdir, "Plugins"))) { Directory.CreateDirectory(Path.Combine(currdir, "Plugins")); }

        List<IPlugin> plugins = [];
        foreach (string file in Directory.GetFiles(Path.Combine(currdir, "Plugins"), "*.dll"))
        {
            if (file.Contains("ignore"))
                continue;
            var assemlby = Assembly.LoadFile(file);
            if (assemlby == null)
                continue;
            var type = assemlby.GetType("Plugin.Plugin");
            if (type == null)
                continue;
            IPlugin? iPlugin = (IPlugin?)Activator.CreateInstance(type);
            if (iPlugin == null)
                continue;
            plugins.Add(iPlugin);
        }
        plugins = plugins.OrderBy(x => x.Priority).ToList();
        foreach (IPlugin iPlugin in plugins)
        {
            if (!pluginsList.ContainsKey(iPlugin.Name))
            {
                PluginInit(iPlugin);
                pluginsList.Add(iPlugin.Name, iPlugin);
            }
        }
    }

    public static List<bool> DemuxDataReceived(Guid ClientNumb,byte[] receivedData)
    {
        List<bool> boolret = [];
        foreach (var plugin in pluginsList)
        {
            boolret.Add(plugin.Value.DemuxDataReceived(ClientNumb, receivedData));
        }
        return boolret;
    }

    public static List<bool> DemuxDataReceivedCustom(Guid ClientNumb, byte[] receivedData, string Protoname)
    {
        List<bool> boolret = [];
        foreach (var plugin in pluginsList)
        {
            boolret.Add(plugin.Value.DemuxDataReceivedCustom(ClientNumb, receivedData, Protoname));
        }
        return boolret;
    }

    public static void UnloadPlugins()
    {
        foreach (var plugin in pluginsList)
        {
            plugin.Value.ShutDown();
            plugin.Value.Dispose();
            Console.WriteLine($"Plugin {plugin.Key} is now unloaded!");
        }
        pluginsList.Clear();
    }

    public static void ManualLoadPlugin(string DllName)
    {
        string currdir = Directory.GetCurrentDirectory();
        var assemlby = Assembly.LoadFile(currdir + "/Plugins/" + DllName + ".dll");
        if (assemlby == null)
            return;
        var type = assemlby.GetType("Plugin.Plugin");
        if (type == null)
            return;
        IPlugin? iPlugin = (IPlugin?)Activator.CreateInstance(type);
        if (iPlugin == null)
            return;
        if (!pluginsList.ContainsKey(iPlugin.Name))
        {
            PluginInit(iPlugin);
            pluginsList.Add(iPlugin.Name, iPlugin);
        }
    }

    public static void ManualUnloadPlugin(string pluginname)
    {
        if (!pluginsList.TryGetValue(pluginname, out var plugin))
            return;
        plugin.ShutDown();
        plugin.Dispose();
        pluginsList.Remove(pluginname);
        Console.WriteLine($"Plugin {pluginname} is now unloaded!");
    }

    private static void PluginInit(IPlugin iPlugin)
    {
        iPlugin.Initialize();
        Debug.PrintDebug("New Plugin Loaded" +
            "\nPlugin Name: " + iPlugin.Name +
            "\nPlugin Priority: " + iPlugin.Priority);
    }
}
