using Google.Protobuf;

namespace SharedLib.Shared;

public class Formatters
{
    /// <summary>
    /// Format File Size
    /// <br>.000 B, KB, MB, GB, TB</br>
    /// </summary>
    /// <param name="lsize">Size as ulong</param>
    /// <returns></returns>
    public static string FormatFileSize(ulong lsize)
    {
        double size = lsize;
        int index = 0;
        for (; size > 1024; index++)
            size /= 1024;
        return size.ToString("0.000 " + new[] { "B", "KB", "MB", "GB", "TB" }[index]);
    }

    /// <summary>
    /// Format Message to Upsteam
    /// </summary>
    /// <param name="rawMessage"></param>
    /// <returns></returns>
    public static byte[] FormatUpstream(byte[] rawMessage)
    {
        return BitConverter.GetBytes(FormatLength((uint)rawMessage.Length)).Concat(rawMessage).ToArray();
    }

    /// <summary>
    /// Get The length of the message
    /// </summary>
    /// <param name="length">Lenght as uint</param>
    /// <returns>The Lenght</returns>
    public static uint FormatLength(uint length)
    {
        return ((length & 0x000000ff) << 24) +
               ((length & 0x0000ff00) << 8) +
               ((length & 0x00ff0000) >> 8) +
               ((length & 0xff000000) >> 24);
    }

    /// <summary>
    /// Get the HashChar from SliceId
    /// </summary>
    /// <param name="sliceId">The SliceId</param>
    /// <returns>Hash Char</returns>
    public static char FormatSliceHashChar(string sliceId)
    {
        char[] base32 = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v' };
        byte reversedValue = byte.Parse($"{sliceId[1]}{sliceId[0]}", System.Globalization.NumberStyles.HexNumber);
        bool isEven = reversedValue % 2 == 0;
        int offset = (int)Math.Floor((decimal)reversedValue / 16);
        int halfOffset = isEven ? 0 : 16;
        return base32[offset + halfOffset];
    }

    /// <summary>
    /// Format Protobuf Data
    /// </summary>
    /// <typeparam name="T">The Protobuf Class</typeparam>
    /// <param name="bytes">Message as Bytes</param>
    /// <returns>The Protobuf Message</returns>
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
            Ex.Handler(ex,"Shared_Formatter");
            return default;
        }
    }

    /// <summary>
    /// Format Protobuf Data with No Length
    /// </summary>
    /// <typeparam name="T">The Protobuf Class</typeparam>
    /// <param name="bytes">Message as Bytes</param>
    /// <returns>The Protobuf Message</returns>
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
            Ex.Handler(ex, "Shared_Formatter");
            return default;
        }
    }
}
