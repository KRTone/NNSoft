namespace NNSoft.PL.Api
{
    public interface INativeServiceOperations
    {
        ErrorCode GetServices(out NativeServiceInfo[] serviceInfo);
        ErrorCode StopServices(ref NativeServiceInfo[] serviceInfo);
        ErrorCode StartServices(ref NativeServiceInfo[] serviceInfo);
    }
}
