using System;
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
            services = serviceManager.GetServices();

            Stop = new AsyncCommand<object>((token, param) => serviceManager.StopService(selectedService.Name));

            IsServiceManagerOperationExecuting = true;
        }

        ServiceInfo[] services;
        public ServiceInfo[] Services
        {
            get { return services; }
            set { SetProperty(ref services, value); }
        }

        ServiceInfo selectedService;
        public ServiceInfo SelectedService
        {
            get { return selectedService; }
            set { SetProperty(ref selectedService, value); }
        }

        ICommand Stop { get; }

        public bool IsServiceManagerOperationExecuting { get; set; }
    }
}
