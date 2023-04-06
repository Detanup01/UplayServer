namespace Core.DemuxResponders
{
    public class Custom
    {
        public static void Requests(int ClientNumb, byte[] bytes, string protoname)
        {
            Extra.PluginHandle.DemuxDataReceivedCustom(ClientNumb, bytes, protoname);
            //Console.WriteLine(ClientNumb + " " + bytes.Length + " " + Encoding.UTF8.GetString(bytes) + " " + protoname);
            switch (protoname)
            {
                default:
                    break;
            }


        }
    }
}
