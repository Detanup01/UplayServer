using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Storm;

public class stormExport
{
    [UnmanagedCallersOnly(EntryPoint = "StormSdkInitialize", CallConvs = [typeof(CallConvCdecl)])]
    public static void StormSdkInitialize()
    {

    }

    [UnmanagedCallersOnly(EntryPoint = "StormSdkUninitialize", CallConvs = [typeof(CallConvCdecl)])]
    public static void StormSdkUninitialize()
    {

    }

    [UnmanagedCallersOnly(EntryPoint = "StormVersion", CallConvs = [typeof(CallConvCdecl)])]
    public static IntPtr StormVersion()
    {
        return Marshal.StringToHGlobalAnsi("2.5.6");
    }

    [UnmanagedCallersOnly(EntryPoint = "StormInitialize", CallConvs = [typeof(CallConvCdecl)])]
    public static bool StormInitialize(IntPtr onLocalPeerDescriptorUpdated, IntPtr applicationId, IntPtr compatibilityId, IntPtr persistentDataPath, IntPtr argumentsKeys, IntPtr argumentsValues, int argumentsSize)
    {
        LocalPeerDescriptorUpdatedHandler localPeerDescriptorUpdatedHandler = Marshal.GetDelegateForFunctionPointer<LocalPeerDescriptorUpdatedHandler>(onLocalPeerDescriptorUpdated);
        string? ApplicationId = Marshal.PtrToStringAnsi(applicationId);
        string? CompatibilityId = Marshal.PtrToStringAnsi(compatibilityId);
        string? PersistentDataPath = Marshal.PtrToStringAnsi(persistentDataPath);


        // Logic for initializing the Storm Engine
        bool initializationSuccess = false;

        // Perform initialization steps
        try
        {
            // Example logic for initializing the engine
            // This would typically involve calling into unmanaged code or performing necessary setup
            // For the sake of this example, we'll simulate initialization

            // Example: Check if the application ID and compatibility ID are valid
            if (string.IsNullOrEmpty(ApplicationId) || string.IsNullOrEmpty(CompatibilityId))
            {
                throw new ArgumentException("Invalid application or compatibility ID");
            }

            // Example: Perform some setup with the persistent data path
            if (string.IsNullOrEmpty(PersistentDataPath))
            {
                throw new ArgumentException("Invalid persistent data path");
            }

            // Example: Process command-line arguments
            for (int i = 0; i < argumentsSize; i++)
            {
                IntPtr keyPtr = Marshal.ReadIntPtr(argumentsKeys, i * IntPtr.Size);
                IntPtr valuePtr = Marshal.ReadIntPtr(argumentsValues, i * IntPtr.Size);
                string? key = Marshal.PtrToStringAnsi(keyPtr);
                string? value = Marshal.PtrToStringAnsi(valuePtr);

                // Process the key-value pair (for example, storing them in a dictionary)
                // This is just an example, actual implementation may vary
                Console.WriteLine($"Key: {key}, Value: {value}");
            }

            // Example: Call the local peer descriptor updated handler
            // This is just a simulated call, actual implementation may vary
            localPeerDescriptorUpdatedHandler(IntPtr.Zero);

            // If all steps succeed, set initializationSuccess to true
            initializationSuccess = true;
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during initialization
            Console.WriteLine($"Initialization failed: {ex.Message}");
        }

        return initializationSuccess;
    }
}
