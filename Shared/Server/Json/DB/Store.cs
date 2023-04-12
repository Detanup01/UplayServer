namespace SharedLib.Server.Json.DB
{
    public class JStore
    {
        public int Id { get; set; }
        public uint productId { get; set; }
        public string configuration { get; set; }
        public string reference { get; set; }
        public string userBlob { get; set; }
        public int partner { get; set; }
        public List<uint> ownershipAssociations { get; set; }
        public List<uint> associations { get; set; }
    }

    public class JStoreData
    {
        public int Id { get; set; }
        public uint productId { get; set; }
        public int Type { get; set; }
    }
}
