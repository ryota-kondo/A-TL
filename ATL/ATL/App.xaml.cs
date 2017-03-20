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

            NavigationService.NavigateAsync("/MasterDetailMainPage/NavigationPage/MainPage/");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();

            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<MenuPage>();
            Container.RegisterTypeForNavigation<MasterDetailMainPage>();
            Container.RegisterTypeForNavigation<SettingPage>();

            Container.RegisterType<IAllPageModel, AllPageModel>(new ContainerControlledLifetimeManager());

            Container.RegisterTypeForNavigation<MonthPage>();
            Container.RegisterTypeForNavigation<YesterdayPage>();
            Container.RegisterTypeForNavigation<WeekPage>();
            Container.RegisterTypeForNavigation<AllDataPage>();
        }
    }
}
