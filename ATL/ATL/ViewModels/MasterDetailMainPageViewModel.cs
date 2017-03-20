using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ATL.Models;
using Prism.Navigation;

namespace ATL.ViewModels
{
    public class MasterDetailMainPageViewModel : BindableBase
    {
        public ObservableCollection<MenuItem> Menus { get; } = new ObservableCollection<MenuItem>
            {
        new MenuItem
        {
            Title = "今日",
            PageName = "MainPage"
        },
        new MenuItem
        {
            Title = "昨日",
            PageName = "YesterdayPage"
        },
        new MenuItem
        {
            Title = "過去1週間",
            PageName = "WeekPage"
        },
        new MenuItem
        {
            Title = "今月",
            PageName = "MonthPage"
        },
        new MenuItem
        {
            Title = "All data",
            PageName = "AllDataPage"
        },
            };

        private INavigationService NavigationService { get; }
        public MasterDetailMainPageViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
        }

        private bool isPresented;
        public bool IsPresented
        {
            get { return this.isPresented; }
            set { this.SetProperty(ref this.isPresented, value); }
        }

        public async Task PageChangeAsync(MenuItem menuItem)
        {
            await this.NavigationService.NavigateAsync($"NavigationPage/{menuItem.PageName}");
            this.IsPresented = false;
        }
    }
}
