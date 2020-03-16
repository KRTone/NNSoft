using NNSoft.PL.Api;
using NNSoft.PL.Common;
using NNSoft.PL.Common.Exceptions;
using NNSoft.PL.Mappings;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;

namespace NNSoft.PL.Models
{
    class ServiceManager
    {
        readonly INativeServiceOperations nativeServiceOperations;
        readonly IMapper mapper;
        readonly TaskFactory taskFactory;

        public ServiceManager(INativeServiceOperations nativeServiceOperations, IMapper mapper)
        {
            this.nativeServiceOperations = nativeServiceOperations ?? throw new ArgumentNullException(nameof(nativeServiceOperations));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.taskFactory = new TaskFactory(new OrderedTaskScheduler());
        }

        public ServiceInfo[] GetServices()
        {
            nativeServiceOperations.GetServices(out NativeServiceInfo[] nativeServices);
            ServiceInfo[] services = mapper.Map<ServiceInfo[]>(nativeServices);
            return services;
        }

        public Task<object> StopService(ServiceInfo service)
        {
            return taskFactory.StartNew<object>(() =>
            {
                NativeServiceInfo native = mapper.Map<NativeServiceInfo>(service);
                nativeServiceOperations.StopService(native);
                UpdateServiceInfo(service, native);
                return null;
            });
        }
        public Task<object> StartService(ServiceInfo service)
        {
            return taskFactory.StartNew<object>(() =>
            {
                NativeServiceInfo native = mapper.Map<NativeServiceInfo>(service);
                nativeServiceOperations.StartService(native);
                UpdateServiceInfo(service, native);
                return null;
            });
        }

        public async Task<object> RestartService(ServiceInfo service)
        {
            return taskFactory.StartNew<object>(() =>
            {
                NativeServiceInfo nativeServiceInfo = mapper.Map<NativeServiceInfo>(service);
                if (service.State == ServiceState.Running)
                {
                    nativeServiceOperations.StopService(nativeServiceInfo);
                    UpdateServiceInfo(service, nativeServiceInfo);
                }

                nativeServiceOperations.StartService(nativeServiceInfo);
                UpdateServiceInfo(service, nativeServiceInfo);
                return null;
            });
        }

        void ThrowIfError(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.False:
                    throw new NativeException(ExceptionMessageManager.BuildException((int)errorCode), (int)errorCode);
            }
        }

        void UpdateServiceInfo(ServiceInfo serviceInfo, NativeServiceInfo actualInfo)
        {
            serviceInfo.Id = actualInfo.id;
            serviceInfo.State = (ServiceState)actualInfo.state;
        }
    }
}
