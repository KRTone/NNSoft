using System.Collections.Generic;
using System.Collections.ObjectModel;
using NNSoft.PL.Mappings;
using Prism.Mvvm;

namespace NNSoft.PL.ViewModels
{
    public class ServiceViewModel : BindableBase
    {
        public ServiceViewModel(IMapper mapper)
        {
            List<Api.ServiceInfo> services = new List<Api.ServiceInfo>();

            services.Add(new Api.ServiceInfo
            {
                DisplayName = nameof(Api.ServiceInfo.DisplayName),
                Group = nameof(Api.ServiceInfo.Group),
                Name = nameof(Api.ServiceInfo.Name),
                Pid = 1,
                State = 1,
            });
            services.Add(new Api.ServiceInfo
            {
                DisplayName = nameof(Api.ServiceInfo.DisplayName),
                Group = nameof(Api.ServiceInfo.Group),
                Name = nameof(Api.ServiceInfo.Name),
                Pid = 2,
                State = 2,
            });

            this.services = mapper.Map<ObservableCollection<Common.ServiceInfo>>(services);
        }

        ObservableCollection<Common.ServiceInfo> services = new ObservableCollection<Common.ServiceInfo>();
        public ObservableCollection<Common.ServiceInfo> Services
        {
            get { return services; }
            set { SetProperty(ref services, value); }
        }
    }
}
