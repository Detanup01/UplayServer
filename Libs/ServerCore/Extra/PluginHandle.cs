using Core.Extra.Interfaces;
using SharedLib.Shared;
using System.Reflection;

namespace Core.Extra
{
    public class PluginHandle
    {
        private static Dictionary<string, IPlugin> pluginsList = new();
        public static void LoadPlugins()
        {
            string currdir = Directory.GetCurrentDirectory();

            if (!Directory.Exists(Path.Combine(currdir, "Plugins"))) { Directory.CreateDirectory(Path.Combine(currdir, "Plugins")); }

            List<IPlugin> plugins = new();
            foreach (string file in Directory.GetFiles(Path.Combine(currdir, "Plugins"), "*.dll"))
            {
                if (file.Contains("ignore")) { continue; }

                IPlugin iPlugin = (IPlugin)Activator.CreateInstance(Assembly.LoadFile(file).GetType("Plugin.Plugin"));
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
            List<bool> boolret = new();
            foreach (var plugin in pluginsList)
            {
                boolret.Add(plugin.Value.DemuxDataReceived(ClientNumb, receivedData));
            }
            return boolret;
        }

        public static List<bool> DemuxDataReceivedCustom(Guid ClientNumb, byte[] receivedData, string Protoname)
        {
            List<bool> boolret = new();
            foreach (var plugin in pluginsList)
            {
                boolret.Add(plugin.Value.DemuxDataReceivedCustom(ClientNumb, receivedData, Protoname));
            }
            return boolret;
        }

        public static List<bool> PluginsHttpRequest(NetCoreServer.HttpRequest request, NetCoreServer.HttpsSession session)
        {
            List<bool> boolret = new();
            foreach (var plugin in pluginsList)
            {
                boolret.Add(plugin.Value.HttpRequest(request, session));
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
            IPlugin iPlugin = (IPlugin)Activator.CreateInstance(Assembly.LoadFile(currdir + "/Plugins/" + DllName + ".dll").GetType("Plugin.Plugin"));
            if (!pluginsList.ContainsKey(iPlugin.Name))
            {
                PluginInit(iPlugin);
                pluginsList.Add(iPlugin.Name, iPlugin);
            }
        }

        public static void ManualUnloadPlugin(string pluginname)
        {
            if (pluginsList.TryGetValue(pluginname, out var plugin))
            {
                plugin.ShutDown();
                plugin.Dispose();
                pluginsList.Remove(pluginname);
                Console.WriteLine($"Plugin {pluginname} is now unloaded!");
            }
        }

        private static void PluginInit(IPlugin iPlugin)
        {
            iPlugin.Initialize();
            Debug.PrintDebug("New Plugin Loaded" +
                "\nPlugin Name: " + iPlugin.Name +
                "\nPlugin Priority: " + iPlugin.Priority);
        }
    }
}
