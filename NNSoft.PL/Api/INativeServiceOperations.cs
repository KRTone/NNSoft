namespace NNSoft.PL.Api
{
    public interface INativeServiceOperations
    {
        ErrorCode GetServices(out NativeServiceInfo[] serviceInfo);
        ErrorCode StopService(NativeServiceInfo serviceInfo);
        ErrorCode StartService(NativeServiceInfo serviceInfo);
    }
}
