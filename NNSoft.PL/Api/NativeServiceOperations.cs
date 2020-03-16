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
        static extern int _GetServices(
            [In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ArrayMarshaler<NativeServiceInfo>), IidParameterIndex = 0)] NativeServiceInfo[] serviceInfo);

        [DllImport(assemblyLocation, EntryPoint = "_GetServiceCount", CallingConvention = CallingConvention.Cdecl)]
        static extern int _GetServiceCount();

        [DllImport(assemblyLocation, EntryPoint = "_StartService", CallingConvention = CallingConvention.Cdecl)]
        static extern int _StartService([In, Out] NativeServiceInfo serviceInfo);

        [DllImport(assemblyLocation, EntryPoint = "_StopService", CallingConvention = CallingConvention.Cdecl)]
        static extern int _StopServices([In, Out]NativeServiceInfo serviceInfo);

        public ErrorCode GetServices(out NativeServiceInfo[] serviceInfoes)
        {
            int count = _GetServiceCount();
            serviceInfoes = new NativeServiceInfo[count];
            for (int i = 0; i < serviceInfoes.Length; i++)
            {
                if (serviceInfoes[i] is null)
                    serviceInfoes[i] = new NativeServiceInfo();
            }
            return (ErrorCode)_GetServices(serviceInfoes);
        }

        public ErrorCode StartService(NativeServiceInfo serviceInfo) => (ErrorCode)_StartService(serviceInfo);

        public ErrorCode StopService(NativeServiceInfo serviceInfo) => (ErrorCode)_StopServices(serviceInfo);
    }
}
