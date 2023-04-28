using Core.Extra.Interfaces;
using SharedLib.Shared;
using System.Net.Security;
using System.Reflection;

namespace Core.Extra
{
    public class PluginHandle
    {
        public static void LoadPlugins()
        {
            string currdir = Directory.GetCurrentDirectory();

            if (!Directory.Exists(Path.Combine(currdir, "Plugins"))) { Directory.CreateDirectory(Path.Combine(currdir, "Plugins")); }

            foreach (string file in Directory.GetFiles(Path.Combine(currdir, "Plugins"), "*.dll"))
            {
                if (file.Contains("ignore")) { continue; }

                IPlugin iPlugin = (IPlugin)Activator.CreateInstance(Assembly.LoadFile(file).GetType("Plugin.Plugin"));
                if (pluginsList.ContainsKey(iPlugin.Name))
                {
                    Console.WriteLine("Plugin already loaded?");
                    iPlugin.ShutDown();
                    iPlugin.Dispose();
                }
                else
                {
                    PluginInit(iPlugin);
                    pluginsList.Add(iPlugin.Name, new PluginInfos
                    {
                        PluginPath = file,
                        Plugin = iPlugin
                    });
                }
            }
        }

        public static void DemuxDataReceived(Guid ClientNumb,byte[] receivedData)
        {
            foreach (var plugin in pluginsList)
            {
                plugin.Value.Plugin.DemuxDataReceived(ClientNumb, receivedData);
            }
        }

        public static void DemuxDataReceivedCustom(Guid ClientNumb, byte[] receivedData, string Protoname)
        {
            foreach (var plugin in pluginsList)
            {
                plugin.Value.Plugin.DemuxDataReceivedCustom(ClientNumb, receivedData, Protoname);
            }
        }

        public static void PluginsHttpRequest(NetCoreServer.HttpRequest request, NetCoreServer.HttpsSession session)
        {
            foreach (var plugin in pluginsList)
            {
                plugin.Value.Plugin.HttpRequest(request, session);
            }
        }

        public static void UnloadPlugins()
        {
            foreach (var plugin in pluginsList)
            {
                plugin.Value.Plugin.ShutDown();
                plugin.Value.Plugin.Dispose();
                Console.WriteLine($"Plugin {plugin.Key} is now unloaded!");
            }
        }

        public static void ManualLoadPlugin(string DllName)
        {

            string currdir = Directory.GetCurrentDirectory();
            IPlugin iPlugin = (IPlugin)Activator.CreateInstance(Assembly.LoadFile(currdir + "/Plugins/" + DllName + ".dll").GetType("Plugin.Plugin"));
            if (pluginsList.ContainsKey(iPlugin.Name))
            {
                Console.WriteLine("Plugin already loaded?");
                iPlugin.ShutDown();
                iPlugin.Dispose();
            }
            else
            {
                PluginInit(iPlugin);
                pluginsList.Add(iPlugin.Name, new PluginInfos
                {
                    PluginPath = currdir + "/Plugins/" + DllName + ".dll",
                    Plugin = iPlugin
                });
            }
        }

        public static void ManualUnLoadPlugin(string pluginname)
        {
            if (pluginsList.TryGetValue(pluginname, out var plugin))
            {
                plugin.Plugin.ShutDown();
                plugin.Plugin.Dispose();
                pluginsList.Remove(pluginname);
                Console.WriteLine($"Plugin {pluginname} is now unloaded!");
            }
        }

        private static void PluginInit(IPlugin iPlugin)
        {
            iPlugin.Initialize();
            Debug.PrintDebug("New Plugin Loaded" +
                "\nPlugin Name: " + iPlugin.Name +
                "\nPlugin Priority: " + iPlugin.Priority +
                "\nPlugin Version: " + iPlugin.PluginExtra.Version +
                "\nPlugin Mode: " + iPlugin.PluginExtra.Mode +
                "\nPlugin Type: " + iPlugin.PluginExtra.PluginType +
                "\nPlugin Dependencies Count: " + iPlugin.PluginExtra.dependencies?.Count);
        }

        internal class PluginInfos
        {
            public string PluginPath;
            public IPlugin Plugin;
        }
        private static Dictionary<string, PluginInfos> pluginsList = new Dictionary<string, PluginInfos>();
    }
}
