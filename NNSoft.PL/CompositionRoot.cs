using NNSoft.PL.Mappings;
using NNSoft.PL.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using Unity;

namespace NNSoft.PL
{
    class CompositionRoot : IModule
    {
        public CompositionRoot(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(nameof(ServiceView), typeof(ServiceView));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            IUnityContainer container = containerRegistry.GetContainer();
            container.RegisterType<IMapper, Mapper>();
        }
    }
}
