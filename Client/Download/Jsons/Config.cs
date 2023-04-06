namespace Downloader
{
    class Config
    {
        public string? DownloadDirectory { get; set; }
        public string? VerifyBinPath { get; set; }
        // Made use of MManager, but warn(?) if not run as admin,
        // Default off!
        public bool InstallDependencies { get; set; } = false;
        public bool Verify { get; set; } = true;
        public bool UsingFileList { get; set; } = false;
        public bool UsingOnlyFileList { get; set; } = false;

        public string? ManifestId { get; set; }
        public uint ProductId { get; set; }
        public string? ProductManifest { get; set; }
        public bool DownloadAsChunks { get; set; } = false;
        public List<Uplay.Download.File>? FilesToDownload { get; set; }

    }
}
