using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace upc_r2.Exports;

internal class Avatar
{
    [UnmanagedCallersOnly(EntryPoint = "UPC_AvatarFree", CallConvs = [typeof(CallConvCdecl)])]
    public static int UPC_AvatarFree(IntPtr inContext, IntPtr inImageRGBA)
    {
        Basics.Log(nameof(UPC_AvatarFree), [inContext, inImageRGBA]);
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
            /*
            var cbList = Main.GlobalContext.Callbacks.ToList();
            cbList.Add(new(inCallback, inCallbackData, 0));
            Main.GlobalContext.Callbacks = cbList.ToArray();*/
            /*
            Basics.Log(nameof(UPC_AvatarGet), new object[] { "Loading avatar" });
            byte[] avatarBytes = new byte[0];
            int AvSize = 0;
            switch (size)
            {
                case UPC_AvatarSize.UPC_AvatarSize_64x64:
                    avatarBytes = File.ReadAllBytes(Basics.GetCuPath() + "/avatars/default_64.png");
                    AvSize = 16384;
                    break;
                case UPC_AvatarSize.UPC_AvatarSize_128x128:
                    avatarBytes = File.ReadAllBytes(Basics.GetCuPath() + "/avatars/default_128.png");
                    AvSize = 65536;
                    break;
                case UPC_AvatarSize.UPC_AvatarSize_256x256:
                    avatarBytes = File.ReadAllBytes(Basics.GetCuPath() + "/avatars/default_256.png");
                    AvSize = 262144;
                    break;
                default:
                    break;
            }
            if (avatarBytes.Length > AvSize)
                Basics.Log(nameof(UPC_AvatarGet), new object[] { "Avatar size is bigger than Length! What we should do?" });

            int Remaining = AvSize - avatarBytes.Length;
            Basics.Log(nameof(UPC_AvatarGet), new object[] { "Leftover bytes are here, we add 0x00 in here! Remaining: ", Remaining });
            while (Remaining != 0)
            {
                avatarBytes = avatarBytes.Append((byte)0x00).ToArray();
                Remaining--;
            }
            Basics.Log(nameof(UPC_AvatarGet), new object[] { "MARSHAL Create!" });
            IntPtr iptr = Marshal.AllocHGlobal(sizeof(byte) * avatarBytes.Length);
            Basics.Log(nameof(UPC_AvatarGet), new object[] { "MARSHAL Iptr: ", iptr });
            Marshal.Copy(avatarBytes, 0, iptr, avatarBytes.Length);
            Basics.Log(nameof(UPC_AvatarGet), new object[] { "MARSHAL COPY!" });
            Marshal.WriteIntPtr(outImageRGBA, 0, iptr);
            Basics.Log(nameof(UPC_AvatarGet), new object[] { "CHECK!", outImageRGBA, iptr });
            //Reding from it because verify!
            IntPtr source = Basics.IntPtrToStruct<IntPtr>(outImageRGBA);
            Basics.Log(nameof(UPC_AvatarGet), new object[] { "source: ", source });
            byte[] array = new byte[AvSize];
            Marshal.Copy(source, array, 0, AvSize);
            File.WriteAllBytes(Basics.GetCuPath() + "/avatars/test_.png", array);
            Basics.Log(nameof(UPC_AvatarGet), new object[] { "CHECK DONE!" });
            +*/
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
        var mem = Marshal.AllocHGlobal(1);
        Marshal.WriteByte(mem, 1);
        Marshal.WriteIntPtr(isBlackListed, 0, mem);
        return 0;
    }
}
