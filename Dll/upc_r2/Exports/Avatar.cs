using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports;

internal class Avatar
{
    static Dictionary<IntPtr, int> PtrToSize = [];


    [UnmanagedCallersOnly(EntryPoint = "UPC_AvatarFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_AvatarFree(IntPtr inContext, IntPtr inImageRGBA)
    {
        Basics.Log(nameof(UPC_AvatarFree), [inContext, inImageRGBA]);
        if (inImageRGBA == IntPtr.Zero)
            return 0;
        if (PtrToSize.TryGetValue(inImageRGBA, out int size))
        {

        }
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_AvatarGet", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_AvatarGet(IntPtr inContext, IntPtr inOptUserIdUtf8, uint inSize, IntPtr outImageRGBA, IntPtr inCallback, IntPtr inCallbackData)
    {
        try
        {
            Basics.Log(nameof(UPC_AvatarGet), [inContext, inOptUserIdUtf8, inSize, outImageRGBA, inCallback, inCallbackData]);
            string? userId = Marshal.PtrToStringUTF8(inOptUserIdUtf8);
            if (userId == null)
                return -1;
            UPC_AvatarSize size = (UPC_AvatarSize)inSize;
            Basics.Log(nameof(UPC_AvatarGet), [userId, size]);
            // 16384 = 64
            // 65536 = 128
            // 262144 = 256
            int avatarSize = ((size != UPC_AvatarSize.UPC_AvatarSize_64x64) ? ((size != UPC_AvatarSize.UPC_AvatarSize_128x128) ? 262144 : 65536) : 16384);
            Basics.Log(nameof(UPC_AvatarGet), ["Max Avatar Size", size]);
            string myUserAvatar = Path.Combine(Basics.GetCuPath(), "avatars", $"{userId}_{inSize}.png");
            string defaultUserAvatar = Path.Combine(Basics.GetCuPath(), "avatars", $"default_{inSize}.png");
            if (!File.Exists(myUserAvatar) && !File.Exists(defaultUserAvatar))
                return -1;
            FileInfo? myFile = null;
            if (File.Exists(myUserAvatar))
            {
                Basics.Log(nameof(UPC_AvatarGet), ["Using User Avatar"]);
                myFile = new FileInfo(myUserAvatar);
            }
            if (File.Exists(defaultUserAvatar))
            {
                Basics.Log(nameof(UPC_AvatarGet), ["Using Default Avatar"]);
                myFile = new FileInfo(defaultUserAvatar);
            }
            if (myFile == null)
                return -1;
            if (myFile.Length > avatarSize)
                return -1;
            byte[] avatarBuffer = new byte[avatarSize];
            var stream = myFile.OpenRead();
            Basics.Log(nameof(UPC_AvatarGet), ["Start Reading | Here might crash :("]);
            stream.ReadExactly(avatarBuffer, 0, (int)myFile.Length);
            stream.Close();
            Main.GlobalContext.Callbacks.Add(new(inCallback, inCallbackData, 0));
            PtrToSize.Add(outImageRGBA, avatarBuffer.Length);
            Marshal.Copy(outImageRGBA, avatarBuffer, 0, avatarBuffer.Length);
        }
        catch (Exception ex)
        {
            Basics.Log(nameof(UPC_AvatarGet), ["Exception: ",  ex]);
        }
        return 0;
    }


    [UnmanagedCallersOnly(EntryPoint = "UPC_BlacklistAdd", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_BlacklistAdd(IntPtr inContext, IntPtr inUserIdUtf8, IntPtr inOptCallback, IntPtr inOptCallbackData)
    {
        Basics.Log(nameof(UPC_BlacklistAdd), [inContext, inUserIdUtf8, inOptCallback, inOptCallbackData]);
        return 0;
    }


    [UnmanagedCallersOnly(EntryPoint = "UPC_BlacklistHas", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_BlacklistHas(IntPtr inContext, IntPtr inUserIdUtf8)
    {
        Basics.Log(nameof(UPC_BlacklistHas), [inContext, inUserIdUtf8]);
        return 0;
    }

    [UnmanagedCallersOnly(EntryPoint = "UPC_BlacklistHas_Extended", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_BlacklistHas_Extended(IntPtr inContext, IntPtr inUserIdUtf8, IntPtr isBlackListed)
    {
        Basics.Log(nameof(UPC_BlacklistHas_Extended), [inContext, inUserIdUtf8]);
        Marshal.WriteByte(isBlackListed, 0);
        return 0;
    }
}
