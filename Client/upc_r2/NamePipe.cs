using Google.Protobuf;
using System.IO.Pipes;
using SharedLib.Shared;
using static upc_r2.Basics;

namespace upc_r2
{
    public class NamePipe
    {

        public static void NamePipeReqRsp(Uplay.Demux.Upstream upstream, out Uplay.Uplaydll.Rsp rsp)
        {
            rsp = new();
            try
            {
                var pipeClient = new NamedPipeClientStream(".", "custom_r2_pipe", PipeDirection.InOut);
                byte[] buffer = new byte[4];
                pipeClient.Connect(1000);
                Log("NamePipeReqRsp", new object[] { "custom_r2_pipe IsConnected!" });
                if (pipeClient.IsConnected)
                {
                    var push = Formatters.FormatUpstream(upstream.ToByteArray());
                    pipeClient.Write(push);
                    pipeClient.Flush();
                    int count = pipeClient.Read(buffer);
                    if (count == 4)
                    {
                        var _InternalReadedLenght = Formatters.FormatLength(BitConverter.ToUInt32(buffer, 0));
                        var _InternalReaded = new byte[(int)_InternalReadedLenght];
                        while (pipeClient.IsConnected)
                        {
                            pipeClient.Read(_InternalReaded);
                            var downstream = Formatters.FormatDataNoLength<Uplay.Demux.Downstream>(_InternalReaded);
                            if (downstream != null)
                            {
                                if (downstream.Response.ServiceRsp != null)
                                {
                                    Log("NamePipeReqRsp", new object[] { "Success? ", downstream.Response.ServiceRsp.Success });
                                    rsp = Uplay.Uplaydll.Rsp.Parser.ParseFrom(downstream.Response.ServiceRsp.Data.ToArray());
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Log("NamePipeReqRsp", new object[] { "Socket not connected" });
                }
                pipeClient.Dispose();
            }
            catch (Exception ex)
            {
                Log("NamePipeReqRsp", new object[] { ex.ToString() });
            }

        }
    }
}
