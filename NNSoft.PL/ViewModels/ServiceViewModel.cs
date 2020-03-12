using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NNSoft.PL.Mappings;
using Prism.Mvvm;

namespace NNSoft.PL.ViewModels
{
    public class ServiceViewModel : BindableBase
    {
        public ServiceViewModel(IMapper mapper, Api.IServiceOperations serviceOperations)
        {
            List<Api.ServiceInfo> services = new List<Api.ServiceInfo>();
            serviceOperations.GetServices(out Api.ServiceInfo[] nativeServices);

            this.services = mapper.Map<ObservableCollection<Common.ServiceInfo>>(nativeServices);
        }

        ObservableCollection<Common.ServiceInfo> services = new ObservableCollection<Common.ServiceInfo>();
        public ObservableCollection<Common.ServiceInfo> Services
        {
            get { return services; }
            set { SetProperty(ref services, value); }
        }
    }
}
