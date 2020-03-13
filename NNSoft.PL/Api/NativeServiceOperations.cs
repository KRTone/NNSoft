using System;
using System.Runtime.InteropServices;

namespace NNSoft.PL.Api
{
    class NativeServiceOperations : INativeServiceOperations
    {
        [DllImport(@"NNSoft.BLL.dll", EntryPoint = "GetServices", CallingConvention = CallingConvention.Cdecl)]
        static extern int _GetServices(
            [In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ArrayMarshaler<NativeServiceInfo>), IidParameterIndex = 0)] NativeServiceInfo[] serviceInfo);

        [DllImport(@"NNSoft.BLL.dll", EntryPoint = "GetServiceCount", CallingConvention = CallingConvention.Cdecl)]
        static extern int _GetServiceCount();

        [DllImport(@"NNSoft.BLL.dll", EntryPoint = "StartServices", CallingConvention = CallingConvention.Cdecl)]
        static extern int _StartServices([In, Out] NativeServiceInfo serviceInfo);

        [DllImport(@"NNSoft.BLL.dll", EntryPoint = "StopServices", CallingConvention = CallingConvention.Cdecl)]
        static extern int _StopServices([In, Out] NativeServiceInfo serviceInfo);

        public ErrorCode GetServices(out NativeServiceInfo[] serviceInfoes)
        {
            int count = _GetServiceCount();
            serviceInfoes = new NativeServiceInfo[count];
            return (ErrorCode)_GetServices(serviceInfoes);
        }

        public ErrorCode StartServices(NativeServiceInfo serviceInfo) => (ErrorCode)_StartServices(serviceInfo);

        public ErrorCode StopServices(NativeServiceInfo serviceInfo) => (ErrorCode)_StopServices(serviceInfo);
    }
}
