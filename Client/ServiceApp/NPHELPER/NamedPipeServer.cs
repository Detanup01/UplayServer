using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.NPHELPER
{
    internal class NamedPipeServer
    {
        private byte[] buffer = new byte[65535];

        private bool connectedOrWaiting;

        private NamedPipeServerStream pipeServer;
        public bool Cancel { get; set; }
        public string Name { get; set; }

        public NamedPipeServer(string name)
        {
            this.connectedOrWaiting = false;
            this.Name = name;
            this.pipeServer = new NamedPipeServerStream(this.Name, PipeDirection.InOut, 1, PipeTransmissionMode.Byte);
            Console.WriteLine("[Server | " + this.Name + "] Starting..");
        }

        public void ExecuteLongRunningTask()
        {
            while (!this.Cancel)
            {
                if (!this.connectedOrWaiting)
                {
                    this.pipeServer.BeginWaitForConnection(delegate (IAsyncResult a)
                    {
                        this.pipeServer.EndWaitForConnection(a);
                    }, null);
                    this.connectedOrWaiting = true;
                }
                if (this.pipeServer.IsConnected)
                {
                    Console.WriteLine("[Server | " + this.Name + "] IsConnected!");
                    int count = this.pipeServer.Read(this.buffer, 0, 65535);
                    if (count > 0)
                    {
                        MemoryStream memoryStream = new MemoryStream(count);
                        memoryStream.Write(this.buffer, 0, count);
                        byte[] BufferDone = memoryStream.ToArray();
                        memoryStream.Dispose();
                        memoryStream.Close();
                        string ReadedAsCoolBytes = BitConverter.ToString(BufferDone);
                        File.AppendAllText("req_as_bytes_" + this.Name.Replace("\\", "") + ".txt", ReadedAsCoolBytes + "\n");
                        Console.WriteLine("[Server | " + this.Name + "] Message got readed!\n" + ReadedAsCoolBytes);
                    }
                }
                Thread.Sleep(10);
            }
            Console.WriteLine("[Server | " + this.Name + "] Is Cancelled!");
            if (this.pipeServer.IsConnected)
            {
                this.pipeServer.Disconnect();
            }
            this.pipeServer.Dispose();
        }
    }
}
