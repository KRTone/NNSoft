using System;
using System.Runtime.InteropServices;

namespace NNSoft.PL.Api
{
    class ServiceOperations : IServiceOperations
    {
        [DllImport(@"NNSoft.BLL.dll", EntryPoint = "GetServices", CallingConvention = CallingConvention.Cdecl)]
        static extern int _GetServices(
            [In, Out, MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ArrayMarshaler<ServiceInfo>), IidParameterIndex = 0)] 
            ServiceInfo[] serviceInfo);

        [DllImport(@"NNSoft.BLL.dll", EntryPoint = "GetServiceCount", CallingConvention = CallingConvention.Cdecl)]
        static extern int _GetServiceCount();

        [DllImport(@"NNSoft.BLL.dll", EntryPoint = "StartServices", CallingConvention = CallingConvention.Cdecl)]
        static extern int _StartServices(ref ServiceInfo[] serviceInfo);

        [DllImport(@"NNSoft.BLL.dll", EntryPoint = "StopServices", CallingConvention = CallingConvention.Cdecl)]
        static extern int _StopServices(ref ServiceInfo[] serviceInfo);

        public ErrorCode GetServices(out ServiceInfo[] serviceInfoes)
        {
            int count = _GetServiceCount();
            serviceInfoes = new ServiceInfo[count];
            return (ErrorCode)_GetServices(serviceInfoes);
        }

        public ErrorCode StartServices(ref ServiceInfo[] serviceInfo) => (ErrorCode)_StartServices(ref serviceInfo);

        public ErrorCode StopServices(ref ServiceInfo[] serviceInfo) => (ErrorCode)_StopServices(ref serviceInfo);
    }
}
