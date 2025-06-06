﻿using System.Text;
using UbiServices.Others;

namespace ClientTester
{
    internal class ServiceTester
    {
        static List<Task> tasks = new List<Task>();
        public static async void Run()
        {
            tasks.Add(AssetPath());
            await Task.WhenAll(tasks);
            Console.WriteLine("ServiceTester Done!");
        }

        public static Task AssetPath()
        {
            var readmecontent = Asset.GetAssetPath("readme.txt");
            if (readmecontent == null)
            {
                return Task.CompletedTask;
            }
            string readme = Encoding.Default.GetString(readmecontent);

            if (readme == "")
            {


            }

            return Task.CompletedTask;
        }
    }
}
