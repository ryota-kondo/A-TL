using ATL.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATL.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private IAllPageModel _model;

        private string _testLabel;
        public string TestLabel
        {
            get { return _testLabel; }
            set { this.SetProperty(ref this._testLabel, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand StartSACommand { get; private set; }
        public DelegateCommand StopSACommand { get; private set; }
        public DelegateCommand ListViewCommand { get; private set; }

        public MainPageViewModel(IAllPageModel model)
        {
            this._model = model;

            StartSACommand = new DelegateCommand(() => _model.StatService.StartService());
            StopSACommand = new DelegateCommand(() => _model.StatService.StopService());
            ListViewCommand = new DelegateCommand(() => TestLabel = _model.GetDbString());
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
