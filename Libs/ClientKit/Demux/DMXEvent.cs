using Uplay.Demux;

namespace ClientKit.Demux
{
    public class DMXEventArgs : EventArgs
    {
        public DMXEventArgs(DataMessage data)
        {
            Data = data;
        }
        public DataMessage Data { get; set; }
    }
}
