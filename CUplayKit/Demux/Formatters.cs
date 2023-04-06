using Google.Protobuf;
using System.Reflection.Metadata;

namespace CUplayKit.Demux
{
    public class Formatters
    {
        public static string FormatFileSize(ulong lsize)
        {
            double size = lsize;
            int index = 0;
            for (; size > 1024; index++)
                size /= 1024;
            return size.ToString("0.000 " + new[] { "B", "KB", "MB", "GB", "TB" }[index]);
        }

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


        public static char FormatSliceHashChar(string sliceId)
        {
            char[] base32 = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v' };
            byte reversedValue = byte.Parse($"{sliceId[1]}{sliceId[0]}", System.Globalization.NumberStyles.HexNumber);
            bool isEven = reversedValue % 2 == 0;
            int offset = (int)Math.Floor((decimal)reversedValue / 16);
            int halfOffset = isEven ? 0 : 16;
            return base32[offset + halfOffset];
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
                InternalEx.WriteEx(ex);
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
                InternalEx.WriteEx(ex);
                return default;
            }
        }
    }
}
