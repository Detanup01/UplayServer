using Newtonsoft.Json;
using SharedLib.Shared;

namespace ServerCore.Json;

public class CDKey
{
    public static string GenerateKey(uint productId, bool isUsed = true)
    {
        bool IsSuccesGen = false;
        string gen = string.Empty;

        //This should work
        int i = 0; //exit counter
        while (!IsSuccesGen)
        {
            if (i == 10)
            {
                //10 time it generated shit, we make an error
                Debug.PWDebug("We tried generating keys 10 times and we still failed!", "GenerateKey");
            }

            gen = Generate();

            if (IfCDKeyExist(gen))
                IsSuccesGen = !IfCDKeyUsed(gen);
            else
                IsSuccesGen = true;

            i++;
        }
       
        JCDKEY key = new()
        {
            IsUsed = isUsed,
            Key = gen,
            ProductId = productId
        };
        AddKey(key);
        return gen;
    }

    public static bool IfCDKeyUsed(string Key)
    {
        List<JCDKEY> list = new();
        if (File.Exists("ServerFiles/CacheFiles/cdkeys.json"))
            list = JsonConvert.DeserializeObject<List<JCDKEY>>(File.ReadAllText("ServerFiles/CacheFiles/cdkeys.json"))!;
        if (list.Where(x=>x.Key == Key && !x.IsUsed).Any())
            return false;

        return true;
    }

    public static bool IfCDKeyExist(string Key)
    {
        List<JCDKEY> list = new();
        if (File.Exists("ServerFiles/CacheFiles/cdkeys.json"))
            list = JsonConvert.DeserializeObject<List<JCDKEY>>(File.ReadAllText("ServerFiles/CacheFiles/cdkeys.json"))!;
        if (list.Where(x => x.Key == Key).Any())
            return true;

        return false;
    }

    public static void AddKey(JCDKEY Key)
    {
        List<JCDKEY> list = new();
        if (File.Exists("ServerFiles/CacheFiles/cdkeys.json"))
            list = JsonConvert.DeserializeObject<List<JCDKEY>>(File.ReadAllText("ServerFiles/CacheFiles/cdkeys.json"))!;
        list.Add(Key);
        File.WriteAllText("ServerFiles/CacheFiles/cdkeys.json", JsonConvert.SerializeObject(list));
    }

    public static string Generate()
    {
        return $"{Randoming(3)}-{Randoming(4)}-{Randoming(4)}-{Randoming(4)}-{Randoming(4)}";
    }

    static string Randoming(int lenght)
    {
        Random random = new Random();
        string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string ran = string.Empty;

        for (int i = 0; i < lenght; i++)
        {
            int x = random.Next(str.Length);
            ran = ran + str[x];
        }

        return ran;
    }

    public class JCDKEY
    {
        public uint ProductId { get; set; }
        public string Key { get; set; } = string.Empty;
        public bool IsUsed { get; set; }

    }
}