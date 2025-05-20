namespace upc_r1;

// TODO: Move this.
/*
public class NamePipe
{

    public static void NamePipeReqRsp(Uplay.Demux.Upstream upstream, out Uplay.Uplaydll.Rsp rsp)
    {
        rsp = new();
        try
        {
            var pipeClient = new NamedPipeClientStream(".", "custom_r1_pipe", PipeDirection.InOut);
            byte[] buffer = new byte[4];
            pipeClient.Connect(1000);
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
                                Log("NamePipeReqRsp", ["Success? ", downstream.Response.ServiceRsp.Success]);
                                rsp = Uplay.Uplaydll.Rsp.Parser.ParseFrom(downstream.Response.ServiceRsp.Data.ToArray());
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                Log("NamePipeReqRsp", ["Socket not connected"]);
            }
            pipeClient.Dispose();
        }
        catch (Exception ex)
        {
            Log("NamePipeReqRsp", [ex.ToString()]);
        }

    }
}
*/