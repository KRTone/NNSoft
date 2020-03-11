using System.Runtime.InteropServices;

namespace NNSoft.PL.Api
{
    class ServiceOperations : IServiceOperations
    {

        [DllImport("NNSoft.PL", EntryPoint = "GetService", CallingConvention = CallingConvention.Cdecl)]
        static extern ErrorCode _GetService(ref ServiceInfo serviceInfo);

        [DllImport("NNSoft.PL", EntryPoint = "StartServices", CallingConvention = CallingConvention.Cdecl)]
        static extern ErrorCode _StartServices(ref ServiceInfo serviceInfo);

        [DllImport("NNSoft.PL", EntryPoint = "StopServices", CallingConvention = CallingConvention.Cdecl)]
        static extern ErrorCode _StopServices(ref ServiceInfo serviceInfo);

        public ErrorCode GetServices(ref ServiceInfo serviceInfo) => _GetService(ref serviceInfo);

        public ErrorCode StartServices(ref ServiceInfo serviceInfo) => _StartServices(ref serviceInfo);

        public ErrorCode StopServices(ref ServiceInfo serviceInfo) => _StopServices(ref serviceInfo);
    }
}
