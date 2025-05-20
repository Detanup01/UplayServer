namespace upc_r2;
/*
public class NamePipe
{

    public static void NamePipeReqRsp(Uplay.Demux.Upstream upstream, out Uplay.Uplaydll.Rsp rsp)
    {
        rsp = new();
        try
        {
            var pipeClient = new NamedPipeClientStream(".", "custom_r2_pipe", PipeDirection.InOut);
            byte[] buffer = new byte[4];
            pipeClient.Connect(10000);
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
                        pipeClient.ReadExactly(_InternalReaded);
                        var downstream = Formatters.FormatDataNoLength<Uplay.Demux.Downstream>(_InternalReaded);
                        if (downstream != null)
                        {
                            if (downstream.Response.ServiceRsp != null)
                            {
                                //Log("NamePipeReqRsp", new object[] { "Success? ", downstream.Response.ServiceRsp.Success });
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
        catch (TimeoutException)
        {
            Log("NamePipeReqRsp", new object[] { "Timeout!" });
        }
        catch (Exception ex)
        {
            Log("NamePipeReqRsp", new object[] { ex.ToString() });
        }

    }
}
*/