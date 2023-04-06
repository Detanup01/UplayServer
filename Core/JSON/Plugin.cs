namespace Core.JSON
{
    public class Plugin
    {
        public pluginType PluginType { get; set; }
        public string Version { get; set; }
        public string Mode { get; set; }
        public List<string>? dependencies { get; set; } = new();
        public enum pluginType
        {
            Core,
            Http,
            Demux,
            DemuxCustom,
            Extra,
            JSON,
            SQL,
            ServerFiles
        }
    }
}
