using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using NNSoft.PL.Async;
using NNSoft.PL.Common;
using NNSoft.PL.Models;
using Prism.Mvvm;

namespace NNSoft.PL.ViewModels
{
    class ServiceViewModel : BindableBase
    {
        readonly ServiceManager serviceManager;
        public ServiceViewModel(ServiceManager serviceManager)
        {
            this.serviceManager = serviceManager ?? throw new ArgumentNullException(nameof(serviceManager));
            services = new ObservableCollection<ServiceInfo>(serviceManager.GetServices());
            StopServiceCommand = new AsyncCommand<object>((token, service) => serviceManager.StopService(service as ServiceInfo));
            StartServiceCommand = new AsyncCommand<object>((token, service) => serviceManager.StartService(service as ServiceInfo));
            RestartServiceCommand = new AsyncCommand<object>((token, service) => serviceManager.RestartService(service as ServiceInfo));
        }

        ObservableCollection<ServiceInfo> services;
        public ObservableCollection<ServiceInfo> Services
        {
            get { return services; }
            private set { SetProperty(ref services, value); }
        }

        ServiceInfo selectedService;
        public ServiceInfo SelectedService
        {
            get { return selectedService; }
            set { SetProperty(ref selectedService, value); }
        }

        public ICommand StopServiceCommand { get; }
        public ICommand StartServiceCommand { get; }
        public ICommand RestartServiceCommand { get; }
    }
}
