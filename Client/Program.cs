using Google.Protobuf;
using System.IO.Pipes;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using ClientKit.UbiServices.Public;
using ClientKit.Demux;
using ClientKit.Demux.Connection;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var reg = V3.Register("testuser", "testuser", "testuser");
            if (reg != null)
            {
                var UserID = reg.UserId;

            }
            var login = V3.Login("testuser", "testuser");
            if (login != null)
            {
                var ticket = login.Ticket;
                ClientKit.UbiServices.Debug.isDebug = true;
                ClientKit.Demux.Debug.isDebug = true;
                Debug.isDebug = true;
                Socket socket = new();
                socket.PushVersion();
                Console.WriteLine(socket.VersionCheck());
                Console.WriteLine(socket.Authenticate(ticket));
                Console.WriteLine(socket.IsAuthed);
                if (args.Contains("install") || args.Contains("download"))
                {
                    args = args.Append("-ticket").Append(ticket).ToArray();
                    Downloader.Program.Main(args, socket);
                }

                Console.ReadLine();
                socket.Close();
            }

            /*
            var x = callbacktest.getcontext();
            Console.WriteLine("yey context! " + x);
            callbacktest.updatecontext(x);
            Console.WriteLine("update! " +x);
            callbacktest.Use(x);
            Console.WriteLine("use! " + x);
            callbacktest.updatecontext(x);
            Console.WriteLine("update! " + x);
            callbacktest.freecontext(x);
            Console.WriteLine("free! " + x);*/
            /*
            var reg = V3.Register("slejm","slejm","slejm");
            if (reg != null)
            {
                var UserID = reg.UserId;

            }
           

            var login = V3.Login("slejm", "slejm");
            if (login != null)
            {
                var ticket = login.Ticket;
                Debug.isDebug = true;
                Socket socket = new();
                socket.PushVersion();
                socket.VersionCheck();
                Console.WriteLine(socket.Authenticate(ticket));

                OwnershipConnection ow = new(socket);
                var games = ow.GetOwnedGames();
                if (games != null)
                {
                    foreach (var x in games)
                    {
                        Console.WriteLine(x.ToString());

                    }

                }




                Console.ReadLine();

                socket.Close();
            }*/
        }

        /*
         
                     Console.WriteLine("Hello, World!"); 
            new Thread(VTest).Start();
            
            Console.WriteLine("Bye, World!");
         
         */
       
        public static void VTest()
        {
           var pipeServer = new NamedPipeServerStream("custom_r2_pipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte);
            Console.WriteLine("server created");
            //new Thread(BTest).Start();
            pipeServer.WaitForConnection();
            Console.WriteLine("con waited");
            if (pipeServer.IsConnected)
            {
                byte[] buffer = new byte[4];
                int count = pipeServer.Read(buffer, 0, 4);
                if (count == 4)
                {
                    Console.WriteLine("4");
                    var _InternalReadedLenght = FormatLength(BitConverter.ToUInt32(buffer, 0));
                    var _InternalReaded = new byte[(int)_InternalReadedLenght]; 
                    Console.WriteLine(_InternalReadedLenght);
                    int readed = pipeServer.Read(_InternalReaded, 0, (int)_InternalReadedLenght);
                    Console.WriteLine(readed);
                    var upstream = FormatDataNoLength<Uplay.Demux.Upstream>(_InternalReaded);
                    if (upstream != null)
                    {
                        Console.WriteLine("not null");
                        Console.WriteLine(upstream);
                        if (!string.IsNullOrEmpty(upstream.Request.ServiceRequest.Service))
                        {
                            Console.WriteLine(upstream.Request.ServiceRequest.Service);
                            Uplay.Demux.Downstream downstream = new()
                            {
                                Response = new()
                                {
                                    RequestId = ReqId,
                                    ServiceRsp = new()
                                    {
                                        Success = true,
                                        Data = ByteString.CopyFrom(new Uplay.Uplaydll.Rsp() { InstallerRsp = new() { InitInstallerRsp = new() { Success = true } } }.ToByteArray())
                                    }
                                }
                            };
                            pipeServer.Write(FormatUpstream(upstream.ToByteArray()));
                            pipeServer.Flush();
                        }
                    }
                    else
                    {
                        Console.WriteLine("whyy");
                    }
                }
            }
            pipeServer.Disconnect();
            pipeServer.Dispose();
        }

        public static T? FormatData<T>(byte[] bytes) where T : IMessage<T>, new()
        {
            try
            {
                if (bytes == null)
                    return default;

                byte[] buffer = new byte[4];

                using var ms = new MemoryStream(bytes);
                ms.Read(buffer, 0, 4);
                var responseLength = FormatLength(BitConverter.ToUInt32(buffer, 0));
                if (responseLength == 0)
                    return default;

                MessageParser<T> parser = new(() => new T());
                return parser.ParseFrom(ms);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public static T? FormatDataNoLength<T>(byte[] bytes) where T : IMessage<T>, new()
        {
            try
            {
                if (bytes == null)
                    return default;

                MessageParser<T> parser = new(() => new T());
                return parser.ParseFrom(bytes);
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        public static uint ReqId = uint.MinValue;
        public static byte[] FormatUpstream(byte[] rawMessage)
        {
            BlobWriter blobWriter = new(4);
            blobWriter.WriteUInt32BE((uint)rawMessage.Length);
            var returner = blobWriter.ToArray().Concat(rawMessage).ToArray();
            blobWriter.Clear();
            return returner;
        }

        public static uint FormatLength(uint length)
        {
            BlobWriter blobWriter = new(4);
            blobWriter.WriteUInt32BE(length);
            var returner = BitConverter.ToUInt32(blobWriter.ToArray());
            blobWriter.Clear();
            return returner;
        }
    }
}