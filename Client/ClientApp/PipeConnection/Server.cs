using System.IO.Pipes;
using SharedLib.Shared;

namespace ClientApp.PipeConnection
{
    public class PipeServer
    {
        volatile bool Cancel;
        NamedPipeServerStream pipeServer;
        bool connectedOrWaiting;
        public event EventHandler<byte[]>? Readed;
        public PipeServer()
        {
            pipeServer = new NamedPipeServerStream("custom_r2_pipe", PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Byte);
        }

        public void Start()
        {
            while (!Cancel)
            {
                if (!connectedOrWaiting)
                {
                    pipeServer.WaitForConnection();
                    connectedOrWaiting = true;
                }

                if (pipeServer.IsConnected)
                {
                    byte[] buffer = new byte[4];
                    int count = pipeServer.Read(buffer);
                    if (count == 4)
                    {
                        MemoryStream ms = new(count);
                        ms.Write(buffer, 0, count);
                        var BufferDone = ms.ToArray();
                        ms.Dispose();

                        var length = Formatters.FormatLength(BitConverter.ToUInt32(BufferDone, 0));
                        buffer = new byte[length];
                        pipeServer.ReadExactly(buffer);
                        Readed?.Invoke(this,buffer);
                    }
                }
                if (!pipeServer.IsConnected && connectedOrWaiting)
                {
                    connectedOrWaiting = false;
                }
                Thread.Sleep(10);
            }

        }

        public void Send(byte[] buf)
        { 
            pipeServer.Write(buf);
            pipeServer.Flush();
            pipeServer.Disconnect();
        }

        public void Stop()
        {
            Debug.PWDebug("Closed.", "PipeServer");
            Cancel = true;
            Thread.Sleep(10);
            if (pipeServer.IsConnected)
            {
                pipeServer.Disconnect();
            }
            pipeServer.Dispose();
        }
    }
}
