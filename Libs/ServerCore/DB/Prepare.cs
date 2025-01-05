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
        Auth.AddUA("00000000-0000-0000-0000-000000000000", "eGx6bGF3Y3ZwaXJ3Rjl4aFBuWURZZVZuaVoyU29Tdm9iMlQ5dHAzeElwMD0=");
        Auth.AddCurrent("00000000-0000-0000-0000-000000000000", "eGx6bGF3Y3ZwaXJ3Rjl4aFBuWURZZVZuaVoyU29Tdm9iMlQ5dHAzeElwMD0=", TokenType.Ticket);

        //fUser Auth with: fUSER@test:test
        Auth.AddUA("ffffffff-ffff-ffff-ffff-ffff-ffffffffffff", "VDF6UWlacDJvTDdCOE5IcXR3bHNYRE8yeDlvWjkvYzh6M083R0hER0hGaz0=");
        Auth.AddCurrent("ffffffff-ffff-ffff-ffff-ffff-ffffffffffff", "VDF6UWlacDJvTDdCOE5IcXR3bHNYRE8yeDlvWjkvYzh6M083R0hER0hGaz0=", TokenType.Ticket);

        Store.Add(new()
        { 
            Id = 0,
            productId = 0,
            associations = new() { 0 },
            configuration = "",
            ownershipAssociations = new() { 0 },
            partner = 0,
            reference = "0",
            Type = 1,
            userBlob = ""         
        });
    }
}