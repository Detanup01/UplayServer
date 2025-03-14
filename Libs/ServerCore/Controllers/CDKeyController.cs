using Newtonsoft.Json;
using Serilog;
using ServerCore.Models;

namespace ServerCore.Controller;

public static class CDKeyController
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
                Log.Debug("We tried generating keys 10 times and we still failed!", "GenerateKey");
                return string.Empty;
            }

            gen = Generate();

            if (IfCDKeyExist(gen))
                IsSuccesGen = !IfCDKeyUsed(gen);
            else
                IsSuccesGen = true;

            i++;
        }

        CDKey key = new()
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
        List<CDKey> list = new();
        if (File.Exists("ServerFiles/CacheFiles/cdkeys.json"))
            list = JsonConvert.DeserializeObject<List<CDKey>>(File.ReadAllText("ServerFiles/CacheFiles/cdkeys.json"))!;
        if (list.Where(x => x.Key == Key && !x.IsUsed).Any())
            return false;

        return true;
    }

    public static bool IfCDKeyExist(string Key)
    {
        List<CDKey> list = new();
        if (File.Exists("ServerFiles/CacheFiles/cdkeys.json"))
            list = JsonConvert.DeserializeObject<List<CDKey>>(File.ReadAllText("ServerFiles/CacheFiles/cdkeys.json"))!;
        if (list.Where(x => x.Key == Key).Any())
            return true;

        return false;
    }

    public static void AddKey(CDKey Key)
    {
        List<CDKey> list = new();
        if (File.Exists("ServerFiles/CacheFiles/cdkeys.json"))
            list = JsonConvert.DeserializeObject<List<CDKey>>(File.ReadAllText("ServerFiles/CacheFiles/cdkeys.json"))!;
        list.Add(Key);
        File.WriteAllText("ServerFiles/CacheFiles/cdkeys.json", JsonConvert.SerializeObject(list));
    }

    public static string Generate()
    {
        return $"{RandomController.RandomString(3)}-{RandomController.RandomString(4)}-{RandomController.RandomString(4)}-{RandomController.RandomString(4)}-{RandomController.RandomString(4)}";
    }
}
