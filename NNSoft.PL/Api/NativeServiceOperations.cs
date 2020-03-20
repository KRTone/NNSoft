using System;
using System.Runtime.InteropServices;

namespace NNSoft.PL.Api
{
    class NativeServiceOperations : INativeServiceOperations
    {
#if RELEASE
        const string assemblyLocation = @"\Assemblies\NNSoft.BLL.dll";
#elif DEBUG
        const string assemblyLocation = "NNSoft.BLL.dll";
#endif

        [DllImport(assemblyLocation, EntryPoint = "_GetServices", CallingConvention = CallingConvention.Cdecl)]
        static extern int _GetServices(out IntPtr serviceInfo, out uint count);

        [DllImport(assemblyLocation, EntryPoint = "_StartService", CallingConvention = CallingConvention.Cdecl)]
        static extern int _StartService([In, Out] NativeServiceInfo serviceInfo);

        [DllImport(assemblyLocation, EntryPoint = "_StopService", CallingConvention = CallingConvention.Cdecl)]
        static extern int _StopServices([In, Out]NativeServiceInfo serviceInfo);

        public ErrorCode GetServices(out NativeServiceInfo[] serviceInfoes)
        {
            var resultCode = _GetServices(out IntPtr ptr, out uint count);
            serviceInfoes = GetManagedArray<NativeServiceInfo>(ptr, count);
            return (ErrorCode)resultCode;
        }

        public ErrorCode StartService(NativeServiceInfo serviceInfo) => (ErrorCode)_StartService(serviceInfo);

        public ErrorCode StopService(NativeServiceInfo serviceInfo) => (ErrorCode)_StopServices(serviceInfo);

        private T[] GetManagedArray<T>(IntPtr pNativeData, uint count)
        {
            T[] result = new T[count];

            if (IntPtr.Zero == pNativeData)
            {
                return null;
            }

            int elSiz = Marshal.SizeOf<T>();
            for (int i = 0; i < count; i++)
            {
                result[i] = Marshal.PtrToStructure<T>(pNativeData + (elSiz * i));
            }

            return result;
        }
    }
}
