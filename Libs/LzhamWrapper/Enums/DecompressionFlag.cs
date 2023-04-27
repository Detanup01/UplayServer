namespace LzhamWrapper.Enums
{
    [Flags]
    public enum DecompressionFlag
    {
        OutputUnbuffered = 1,
        ComputeAdler32 = 2,
        ReadZlibStream = 4
    }
}
