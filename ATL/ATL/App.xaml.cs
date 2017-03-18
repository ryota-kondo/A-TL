using Prism.Unity;
using ATL.Views;
using ATL.Helpers;
using ATL.Models;
using Microsoft.Practices.Unity;
using Xamarin.Forms;

namespace ATL
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("/NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();

            Container.RegisterTypeForNavigation<MainPage>();

            Container.RegisterType<IAllPageModel, AllPageModel>(new ContainerControlledLifetimeManager());
            Container.RegisterTypeForNavigation<MenuPage>();
        }
    }
}
