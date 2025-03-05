using ServerCore.Models.User;

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
        Auth.AddUA(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), "VDF6UWlacDJvTDdCOE5IcXR3bHNYRE8yeDlvWjkvYzh6M083R0hER0hGaz0=");
        Auth.AddCurrent(Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), "VDF6UWlacDJvTDdCOE5IcXR3bHNYRE8yeDlvWjkvYzh6M083R0hER0hGaz0=", TokenType.Ticket);

        //user@uplayemu.com:user | user
        Auth.AddUA(Guid.Parse("ea66fece-ccce-4fe4-ad60-45298a0ac92a"), "MjA1anpTNmdlbnZveE1ZR0lLby8xT3REcWt3cjhibGcrc2hKRnZkK3JaRT0=");
        Auth.AddCurrent(Guid.Parse("ea66fece-ccce-4fe4-ad60-45298a0ac92a"), "MjA1anpTNmdlbnZveE1ZR0lLby8xT3REcWt3cjhibGcrc2hKRnZkK3JaRT0=", TokenType.Ticket);


        DBUser.Add(new UserLogin()
        {
            UserId = Guid.Parse("ea66fece-ccce-4fe4-ad60-45298a0ac92a"),
            Password = "user",
            Country = "EU",
            DateOfBirth = "2000-01-01",
            Email = "user@uplayemu.com",
            LegalOptinsKey = "-",
            NameOnPlatform = "user",
            PreferredLanguage = "US"
        });
        DBUser.Add(new UserCommon()
        { 
            Name = "user",
            Friends = [],
            IsBanned = false,
            UserId = Guid.Parse("ea66fece-ccce-4fe4-ad60-45298a0ac92a")
        });

        Console.WriteLine(DBUser.Get<UserCommon>("ea66fece-ccce-4fe4-ad60-45298a0ac92a") != null);

        Store.Add(new()
        { 
            ProductId = 0,
            Associations = [0],
            Configuration = "",
            OwnershipAssociations = [0],
            Partner = 0,
            Reference = "0",
            Type = 1,
            UserBlob = ""         
        });
    }
}