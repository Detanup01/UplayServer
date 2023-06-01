using System;
using System.Runtime.InteropServices;
using static upc_r2.Enums;

namespace upc_r2
{
    public class Structures
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Context
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStruct, SizeConst = 1)]
            public Callback[] Callbacks;
            public Config Config;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStruct, SizeConst = 1)]
            public Event[] Events;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Callback
        {
            public Callback(IntPtr fn, IntPtr contextdata, int uarg)
            {
                context_data = contextdata;
                arg = uarg;
                fun = fn;
            }

            [MarshalAs(UnmanagedType.SysInt)]
            public IntPtr fun;
            [MarshalAs(UnmanagedType.I4)]
            public int arg;
            [MarshalAs(UnmanagedType.SysInt)]
            public IntPtr context_data;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct Config
        {
            public InitSaved Saved;
            public uint ProductId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct InitSaved
        {
            public Uplay.Uplaydll.Account account;
            public string savePath;
            public string ubiTicket;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Event
        {
            public Event(UPC_EventType eventType, IntPtr handler, IntPtr optdata)
            {
                EventType = eventType;
                Handler = handler;
                OptData = optdata;
            }

            public UPC_EventType EventType;
            public IntPtr Handler;
            public IntPtr OptData;
        }


        public struct UPC_ContextSettings
        {
            public UPC_ContextSubsystem subsystems;
        }
    }
}
