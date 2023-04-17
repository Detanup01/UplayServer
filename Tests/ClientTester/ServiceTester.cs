using ClientKit.UbiServices.Others;
using System.Text;

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

        public static async Task AssetPath()
        {
            var readmecontent = Asset.GetAssetPath("readme.txt");
            string readme = Encoding.Default.GetString(readmecontent);

            if (readme == "")
            {


            }

            return;
        }
    }
}
