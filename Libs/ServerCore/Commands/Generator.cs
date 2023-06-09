﻿using SharedLib.Server.Json;

namespace Core.Commands
{
    public class Generator
    {
        public static void GenerateCDKey(object obj)
        {
            var args = (string[])obj;
            if (args.Length == 0)
            {
                Console.WriteLine("Use as: !generatecdkey {productId}");
                return;
            }
            var cdkey = args[0];
            var key = CDKey.GenerateKey(uint.Parse(cdkey),false);
            Console.WriteLine($"Key for {cdkey} is: {key}");
        }
    }
}
