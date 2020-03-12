namespace NNSoft.PL.Api
{
    public interface IServiceOperations
    {
        ErrorCode GetServices(out ServiceInfo[] serviceInfo);
        ErrorCode StopServices(ref ServiceInfo[] serviceInfo);
        ErrorCode StartServices(ref ServiceInfo[] serviceInfo);
    }
}
