using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Provider;

using ATL.Helpers;
using Xamarin.Forms;
using System.Threading.Tasks;
using static Android.Manifest;

namespace ATL.Droid
{
    /// <summary>
    /// Serviceの常駐のON/OFFの機能をインジェクションするクラス
    /// </summary>
    class StartService_Droid : IStartService
    {
        private static readonly Context _Context = Forms.Context;
        private readonly Intent _intent = new Intent(_Context, typeof(AggregateService));

        public void StartService()
        {
            if (true)
            {
                isUsageStatsAllowed();
            }
            _Context.StartService(_intent);
        }

        public void StopService()
        {
            _Context.StopService(_intent);
            
        }

        public void isUsageStatsAllowed()
        {
            _Context.StartActivity(new Intent(Settings.ActionUsageAccessSettings));
        }
    }
}