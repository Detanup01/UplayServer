using System.Text.Json;

namespace upc_r2;

public class UPC_Json
{
    public static Root GetRoot()
    {
        if (!File.Exists(Basics.GetCuPath() + "\\upc.json"))
        {
            Root root = new();
            File.WriteAllText(Basics.GetCuPath() + "\\upc.json", JsonSerializer.Serialize(root, JsonSourceGen.Default.Root));
            return root;
        }
        Root? data = JsonSerializer.Deserialize(File.ReadAllText(Basics.GetCuPath() + "\\upc.json"), JsonSourceGen.Default.Root);
        data ??= new();
        return data;
    }

    public class BasicLog
    {
        public bool ReqLog { get; set; }
        public bool RspLog { get; set; }
        public bool UseNamePipeClient { get; set; }
        public uint WaitBetweebUpdate { get; set; } = 20_000;
        public bool LogUpdate { get; set; }
    }


    public class Account
    {
        public string AccountId { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; } = "user@uplayemu.com";
        public string Name { get; set; } = "user";
        public string Password { get; set; } = "user";
        public string Country { get; set; } = "en-US";
        public string Ticket { get; set; } = string.Empty;
        public bool UseTicket { get; set; } = false;
    }

    public class Save
    {
        public string Path { get; set; } = "upc_save";
        public bool UseProductIdInName { get; set; }
        public bool EnableFileDelete { get; set; }
    }

    public class Others
    {
        public string ApplicationId { get; set; } = string.Empty;
        public bool EnableCrossBoot { get; set; }
    }

    public class Product
    {
        public uint ProductId { get; set; }

        // Check Uplay.Uplaydll.ProductType for this.
        // DLC = 2
        // Item = 4
        public uint Type { get; set; }
    }

    public class ChunkIds
    {
        public uint ChunkId { get; set; }
        public string ChunkTag { get; set; } = string.Empty;
    }

    public class Root
    {
        public BasicLog BasicLog { get; set; } = new();
        public Account Account { get; set; } = new();
        public Save Save { get; set; } = new();
        public Others Others { get; set; } = new();
        public List<Product> Products { get; set; } = [new() { ProductId = 0, Type = 4 }];
        public List<uint> AutoProductIds { get; set; } = [];
        public List<ChunkIds> ChunkIds { get; set; } = [];
    }

}
