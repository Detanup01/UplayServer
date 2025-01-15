namespace ServerCore.DB;

public class Prepare
{
    private static readonly string extractPath = Directory.GetCurrentDirectory();
    public static readonly string DatabasePath = extractPath + "\\DataBase\\";
    public static void MakeAll()
    {
        if (!Directory.Exists(extractPath + "\\Database"))
        {
            Directory.CreateDirectory(extractPath + "\\Database");
        }

        //  Other DB Init
        App.Init();

        //0User Auth with: 0USER@test:test
        Auth.AddUA(Guid.Parse("00000000-0000-0000-0000-000000000001"), "eGx6bGF3Y3ZwaXJ3Rjl4aFBuWURZZVZuaVoyU29Tdm9iMlQ5dHAzeElwMD0=");
        Auth.AddCurrent(Guid.Parse("00000000-0000-0000-0000-000000000001"), "eGx6bGF3Y3ZwaXJ3Rjl4aFBuWURZZVZuaVoyU29Tdm9iMlQ5dHAzeElwMD0=", TokenType.Ticket);

        //fUser Auth with: fUSER@test:test
        Auth.AddUA(Guid.Parse("ffffffff-ffff-ffff-ffff-ffff-ffffffffffff"), "VDF6UWlacDJvTDdCOE5IcXR3bHNYRE8yeDlvWjkvYzh6M083R0hER0hGaz0=");
        Auth.AddCurrent(Guid.Parse("ffffffff-ffff-ffff-ffff-ffff-ffffffffffff"), "VDF6UWlacDJvTDdCOE5IcXR3bHNYRE8yeDlvWjkvYzh6M083R0hER0hGaz0=", TokenType.Ticket);

        Store.Add(new()
        { 
            ProductId = 0,
            Associations = new() { 0 },
            Configuration = "",
            OwnershipAssociations = new() { 0 },
            Partner = 0,
            Reference = "0",
            Type = 1,
            UserBlob = ""         
        });
    }
}