namespace Downloader
{
    internal static class Split
    {
        public static List<List<T>> SplitList<T>(this List<T> me, int size = 30)
        {
            var list = new List<List<T>>();
            for (int i = 0; i < me.Count; i += size)
                list.Add(me.GetRange(i, Math.Min(size, me.Count - i)));
            return list;
        }
    }
}