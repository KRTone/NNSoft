namespace NNSoft.PL.Api
{
    interface IServiceOperations
    {
        ErrorCode GetServices(ref ServiceInfo serviceInfo);
        ErrorCode StopServices(ref ServiceInfo serviceInfo);
        ErrorCode StartServices(ref ServiceInfo serviceInfo);
    }
}
