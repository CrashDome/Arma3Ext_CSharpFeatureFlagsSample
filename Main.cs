using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SampleExt
{
    //Optional:  CPPInterop partial class and enum only needed if changing values at runtime
    [Flags]
    internal enum ExtensionFeatureFlags : ulong
    {
        None = 0,
        RVFeature_ContextArgumentsVoidPtr = 1 << 0,
        RVFeature_ContextStackTrace = 1 << 1,
        RVFeature_ContextNoDefaultCall = 1 << 2,
        RVFeature_ArgumentNoEscapeString = 1 << 3,
    }

    internal static partial class CPPInterop
    {
        [LibraryImport("SampleExt_x64.dll", EntryPoint = "SetRVExtensionFeatureFlags")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
        internal static partial void SetFeatureFlags(ulong value);

        [LibraryImport("SampleExt_x64.dll", EntryPoint = "GetRVExtensionFeatureFlags")]
        [UnmanagedCallConv(CallConvs = new[] { typeof(CallConvCdecl) })]
        internal static partial ulong GetFeatureFlags();
    }

    //Minimal Required Implementation of RVExtension and RVExtensionArgs
    public class Main
    {
        [UnmanagedCallersOnly(EntryPoint = "RVExtension")]
        public unsafe static void RVExtension(char* output, uint outputSize, char* func)
        {
            // Get Feature Flags example
            WriteOutput(output, outputSize, $"Feature Flags Value: {CPPInterop.GetFeatureFlags()}");
        }

        [UnmanagedCallersOnly(EntryPoint = "RVExtensionArgs")]
        public unsafe static int RVExtensionArgs(char* output, uint outputSize, char* func, char** argValues, uint argCount)
        {
            // Set Feature Flags example
            CPPInterop.SetFeatureFlags((ulong)ExtensionFeatureFlags.None);
            return 0;
        }

        public static unsafe void WriteOutput(char* buffer, uint outputSize, string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            int length = (int)Math.Min(bytes.Length, outputSize - 1); // Do not overflow the buffer
            Marshal.Copy(bytes, 0, (IntPtr)buffer, length);
            Marshal.WriteByte((nint)buffer, length, 0); // Null-terminate the string
        }
    }
}
