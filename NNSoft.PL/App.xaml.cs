using System.Windows;
using NNSoft.PL.Views;
using Prism.Ioc;
using Prism.Modularity;
using Unity;

namespace NNSoft.PL
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Prism.Unity.PrismApplication
    {
        protected override Window CreateShell()
        {
            base.InitializeModules();
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CompositionRoot>();
        }
    }
}
