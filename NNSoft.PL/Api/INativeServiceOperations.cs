namespace NNSoft.PL.Api
{
    public interface INativeServiceOperations
    {
        ErrorCode GetServices(out NativeServiceInfo[] serviceInfo);
        ErrorCode StopServices(NativeServiceInfo serviceInfo);
        ErrorCode StartServices(NativeServiceInfo serviceInfo);
    }
}
