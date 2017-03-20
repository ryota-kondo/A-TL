using ATL.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ATL.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private readonly IAllPageModel _model;
        private const string SaveFileName = "setting.json";

        private IEnumerable<AppNameAndExecTime> _executeTimes;
        public IEnumerable<AppNameAndExecTime> ExecuteTimesList
        {
            get { return _executeTimes; }
            set { this.SetProperty(ref this._executeTimes, value); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return this.isBusy; }
            set { this.SetProperty(ref this.isBusy, value); }
        }

        private string _onoff;
        public string ONandOFFtext
        {
            get { return this._onoff; }
            set { this.SetProperty(ref this._onoff, value); }
        }

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
            bool Flag = false;
            try
            {
                var data = JsonConvert.DeserializeObject<SettingData>(_model.SaveAndLoad.LoadData(SaveFileName));
                Flag = data.Startup;
            }
            catch (Exception e)
            {
                var data = JsonConvert.SerializeObject(new SettingData(false));
                _model.SaveAndLoad.SaveData(SaveFileName, data);
                StartStop();
            }

            if (Flag)
            {
                var data = JsonConvert.SerializeObject(new SettingData(false));
                _model.SaveAndLoad.SaveData(SaveFileName, data);
                ONandOFFtext = "OFF";
                _model.StatService.StopService();
            }
            else
            {
                var data = JsonConvert.SerializeObject(new SettingData(true));
                _model.SaveAndLoad.SaveData(SaveFileName, data);
                ONandOFFtext = "ON";
                _model.StatService.StartService();
            }
        }

        /// <summary>
        /// 集計した情報を一覧表示
        /// </summary>
        private async void SetList()
        {
            try
            {
                this.IsBusy = true;
                await Task.Delay(1000);
                var t = _model.GetTodayLists();
                ExecuteTimesList = t;
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            SetList();

            var Flag = false;
            try
            {
                var data = JsonConvert.DeserializeObject<SettingData>(_model.SaveAndLoad.LoadData(SaveFileName));
                Flag = data.Startup;
            }
            catch (Exception e)
            {
                var data = JsonConvert.SerializeObject(new SettingData(false));
                _model.SaveAndLoad.SaveData(SaveFileName, data);
                StartStop();
            }

            if (Flag)
            {
                ONandOFFtext = "ON";
                _model.StatService.StartService();
            }
            else
            {
                ONandOFFtext = "OFF";
            }
        }
    }
}
