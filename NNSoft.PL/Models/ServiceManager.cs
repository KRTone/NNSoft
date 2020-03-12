using NNSoft.PL.Api;
using NNSoft.PL.Common;
using NNSoft.PL.Mappings;
using System;
using System.Threading.Tasks;

namespace NNSoft.PL.Models
{
    class ServiceManager
    {
        readonly INativeServiceOperations nativeServiceOperations;
        readonly IMapper mapper;

        public ServiceManager(INativeServiceOperations nativeServiceOperations, IMapper mapper)
        {
            this.nativeServiceOperations = nativeServiceOperations ?? throw new ArgumentNullException(nameof(nativeServiceOperations));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));           
        }

        public ServiceInfo[] GetServices()
        {
            nativeServiceOperations.GetServices(out NativeServiceInfo[] nativeServices);
            ServiceInfo[] services = mapper.Map<ServiceInfo[]>(nativeServices);
            return services;
        }

        public async Task<object> StopService(string serviceName)
        {
            return Task.FromResult<object>(null);
        }
    }
}
