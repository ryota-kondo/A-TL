﻿using ATL.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ATL.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private readonly IAllPageModel _model;

        private IEnumerable<AppNameAndExecTime> _executeTimes;
        public IEnumerable<AppNameAndExecTime> Test1
        {
            get { return _executeTimes; }
            set { this.SetProperty(ref this._executeTimes, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand ToolbarCommand { get; private set; }
        public DelegateCommand StartSACommand { get; private set; }
        public DelegateCommand StopSACommand { get; private set; }
        public DelegateCommand ListViewCommand { get; private set; }

        public MainPageViewModel(IAllPageModel model, INavigationService navigationService)
        {
            this._model = model;

            ToolbarCommand = new DelegateCommand(() => navigationService.NavigateAsync("MenuPage"));
            StartSACommand = new DelegateCommand(() => _model.StatService.StartService());
            StopSACommand = new DelegateCommand(() => _model.StatService.StopService());
            ListViewCommand = new DelegateCommand(SetList);
        }

        private void ToMenuPage()
        {
            
        }

        private void SetList()
        {
            var t = _model.GetTodayLists();
            Test1 = t;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("title"))
                Title = (string)parameters["title"] + " and Prism";
        }
    }
}
