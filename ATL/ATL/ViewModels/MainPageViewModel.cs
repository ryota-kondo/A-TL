using ATL.Helpers;
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
        public IEnumerable<AppNameAndExecTime> ExecuteTimesList
        {
            get { return _executeTimes; }
            set { this.SetProperty(ref this._executeTimes, value); }
        }

        private bool _startFlag = false;

        public DelegateCommand ToolbarCommand { get; private set; }
        public DelegateCommand ToolbarOnOffCommand { get; private set; }
        public DelegateCommand ListViewCommand { get; private set; }

        public MainPageViewModel(IAllPageModel model, INavigationService navigationService)
        {
            this._model = model;

            ToolbarCommand = new DelegateCommand(() => navigationService.NavigateAsync("MenuPage"));
            ToolbarOnOffCommand = new DelegateCommand(() => StartStop());
            ListViewCommand = new DelegateCommand(SetList);
        }

        /// <summary>
        /// 状態に応じて起動と終了を切り替える
        /// </summary>
        private void StartStop()
        {
            if (!_startFlag)
            {
                _startFlag = true;
                _model.StatService.StartService();
            }
            else
            {
                _startFlag = false;
                _model.StatService.StopService();
            }
        }

        /// <summary>
        /// 集計した情報を一覧表示
        /// </summary>
        private void SetList()
        {
            var t = _model.GetTodayLists();
            ExecuteTimesList = t;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            SetList();
        }
    }
}
