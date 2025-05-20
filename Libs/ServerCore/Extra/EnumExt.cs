namespace ServerCore;

public static class EnumExt
{
    public static T[] CopySlice<T>(this T[] source, int index, int length, bool padToLength = false)
    {
        int n = length;
        T[] slice = [];

        if (source.Length < index + length)
        {
            n = source.Length - index;
            if (padToLength)
            {
                slice = new T[length];
            }
        }

        if (slice.Length == 0)
            slice = new T[n];
        Array.Copy(source, index, slice, 0, n);
        return slice;
    }

    public static IEnumerable<T[]> Slices<T>(this T[] source, int count, bool padToLength = false)
    {
        for (var i = 0; i < source.Length; i += count)
            yield return source.CopySlice(i, count, padToLength);
    }


    public static IEnumerable<byte[]> Split(this byte[] value, int bufferLength)
    {
        int countOfArray = value.Length / bufferLength;
        if (value.Length % bufferLength > 0)
            countOfArray++;
        for (int i = 0; i < countOfArray; i++)
        {
            yield return value.Skip(i * bufferLength).Take(bufferLength).ToArray();

        }
    }
}