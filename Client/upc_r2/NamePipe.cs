using Google.Protobuf;
using System.IO.Pipes;
using System.Reflection.Metadata;
using static upc_r2.Basics;

namespace upc_r2
{
    public class NamePipe
    {
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

        public static void NamePipeReqRsp(Uplay.Demux.Upstream upstream, out Uplay.Uplaydll.Rsp rsp)
        {
            rsp = new();
            try
            {
                var pipeClient = new NamedPipeClientStream(".", "custom_r2_pipe", PipeDirection.InOut);
                byte[] buffer = new byte[4];
                pipeClient.Connect();
                Log("SendReq", new object[] { "custom_r2_pipe IsConnected!" });
                if (pipeClient.IsConnected)
                {
                    var push = FormatUpstream(upstream.ToByteArray());
                    Log("SendReq", new object[] { "custom_r2_pipe send bytes:", push.Length });
                    pipeClient.Write(push);
                    pipeClient.Flush();
                    Log("SendReq", new object[] { "flush!" });
                    int count = pipeClient.Read(buffer, 0, 4);
                    if (count == 4)
                    {
                        Log("SendReq", new object[] { "4 readed!" });
                        var _InternalReadedLenght = FormatLength(BitConverter.ToUInt32(buffer, 0));
                        var _InternalReaded = new byte[(int)_InternalReadedLenght];
                        while (pipeClient.IsConnected)
                        {
                            pipeClient.Read(_InternalReaded, 0, (int)_InternalReadedLenght);
                            var downstream = FormatDataNoLength<Uplay.Demux.Downstream>(_InternalReaded);
                            if (downstream != null)
                            {
                                if (downstream.Response.ServiceRsp != null)
                                {
                                    Log("SendReq", new object[] { "Success? ", downstream.Response.ServiceRsp.Success });
                                    rsp = Uplay.Uplaydll.Rsp.Parser.ParseFrom(downstream.Response.ServiceRsp.Data.ToArray());
                                }
                                break;
                            }
                        }
                    }
                }
                pipeClient.Dispose();
            }
            catch (Exception ex)
            {
                Log("SendReq", new object[] { ex.ToString() });
            }

        }
    }
}
