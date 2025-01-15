using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;

namespace Client;

internal class CallbackTest
{
    #region Fields
    private static long s_requestId = 0L;
    private static readonly Dictionary<long, UPC_Callback> s_waitingRequests = new Dictionary<long, UPC_Callback>();
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void UPC_CallbackImpl(int inResult, IntPtr inData);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void UPC_Callback(int inResult);
    //public static IntPtr HandleEventDelegater;
    #endregion

    #region Event/Reguest Handling
    private static long PushRequest(UPC_Callback callback)
    {
        Console.WriteLine("PushRequest " + s_requestId);
        long num2;
        lock (s_waitingRequests)
        {
            s_requestId += 1L;
            num2 = s_requestId;
            s_waitingRequests.Add(num2, callback);
        }
        return num2;
    }
    private static void CancelRequest(long requestId)
    {
        Console.WriteLine("CancelRequest " + requestId);
        lock (s_waitingRequests)
        {
            s_waitingRequests.Remove(requestId);
        }
    }

    // public unsafe static delegate* unmanaged<int, IntPtr, void> _HandleReq;
    private static unsafe void* _HandleReqVoid;
    public unsafe static delegate* unmanaged<int, IntPtr, void> _HandleReq { get => (delegate* unmanaged<int, IntPtr, void>)_HandleReqVoid; set => _HandleReqVoid = (void*)value; }
    [UnmanagedCallersOnly]
    private static void HandleRequestUnMG(int inResult, IntPtr inData)
    {
        Console.WriteLine($"HandleRequest | {inResult}| {inData}");
        long num = inData.ToInt64();
        UPC_Callback? upc_Callback = null;
        lock (s_waitingRequests)
        {
            if (!s_waitingRequests.TryGetValue(num, out upc_Callback))
            {
                throw new Exception(string.Format("Invalid Request ID received {0}, the UPC request should have been not executed but was. Something went wrong", num));
            }
            s_waitingRequests.Remove(num);
        }
        if (upc_Callback != null)
        {
            Console.WriteLine($"HandleRequest | upc_Callback fired!");
            upc_Callback(inResult);
        }
    }

    [InvokeCallback(typeof(UPC_CallbackImpl))]
    private static void HandleRequest(int inResult, IntPtr inData)
    {
        Console.WriteLine($"HandleRequest | {inResult}| {inData}");
        long num = inData.ToInt64();
        UPC_Callback? upc_Callback = null;
        lock (s_waitingRequests)
        {
            if (!s_waitingRequests.TryGetValue(num, out upc_Callback))
            {
                throw new Exception(string.Format("Invalid Request ID received {0}, the UPC request should have been not executed but was. Something went wrong", num));
            }
            s_waitingRequests.Remove(num);
        }
        if (upc_Callback != null)
        {
            Console.WriteLine($"HandleRequest | upc_Callback fired!");
            upc_Callback(inResult);
        }
    }

    #endregion
    #region Imports
    [DllImport("upc_r2_loader", CallingConvention = CallingConvention.Cdecl, EntryPoint = "getcontext")]
    public static extern IntPtr getcontext();


    [DllImport("upc_r2_loader", CallingConvention = CallingConvention.Cdecl, EntryPoint = "freecontext")]
    public static extern int freecontext(IntPtr context);


    [DllImport("upc_r2_loader", CallingConvention = CallingConvention.Cdecl, EntryPoint = "updatecontext")]
    public static extern int updatecontext(IntPtr context);

    [DllImport("upc_r2_loader", CallingConvention = CallingConvention.Cdecl, EntryPoint = "UPC_Init")]
    public static extern int UPC_Init(uint version, int appID);

    [DllImport("upc_r2_loader", CallingConvention = CallingConvention.Cdecl, EntryPoint = "usecontext")]
    public static extern int usecontext(IntPtr context, IntPtr indata, IntPtr indataplus);

    [DllImport("dummydll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ProductListGet")]
    public static extern int ProductListGet(ref IntPtr outProductList);

    [DllImport("dummydll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "ProductListFree")]
    public static extern int ProductListFree(IntPtr outProductList);

    #endregion
    #region struct
    [StructLayout(LayoutKind.Sequential)]
    public struct TestName
    {
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public string nameUtf8;
    }
    #endregion
    public static T IntPtrToStruct<T>(IntPtr ptr) where T : struct
    {
        T? ret = (T?)Marshal.PtrToStructure(ptr, typeof(T));
        if (ret == null)
            return default;
        return (T)ret!;
    }
    public static unsafe void getlist()
    {
        IntPtr ptrProductList = IntPtr.Zero;
        var back = ProductListGet(ref ptrProductList);
        Console.WriteLine("back: " + back);
        Console.WriteLine(ptrProductList);
        TestName upc_ProductList = IntPtrToStruct<TestName>(ptrProductList);
        Console.WriteLine(JsonConvert.SerializeObject(upc_ProductList));
        ProductListFree(ptrProductList);

    }

    public static unsafe void Use(IntPtr context)
    {
        long num = PushRequest(delegate (int inResult)
        {
            Console.WriteLine("push: " + inResult);
        });
        var ptr = Marshal.GetFunctionPointerForDelegate<UPC_CallbackImpl>(new UPC_CallbackImpl(HandleRequest));
        /*
        _HandleReqVoid = 
         _HandleReq = &HandleRequestUnMG;
        IntPtr x = new(_HandleReqVoid);*/
        var resp = usecontext(context, ptr, new IntPtr(num));
        if (resp < 0)
        {
            CancelRequest(num);
        }

    }
}
