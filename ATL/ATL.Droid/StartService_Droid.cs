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
using Android.Content.PM;
using Android.Support.V4.Content;
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
            if (!isUsageStatsAllowed())
            {
                var alert = new AlertDialog.Builder(Forms.Context);
                alert.SetTitle("Infomation");
                alert.SetMessage("計測するためには使用履歴へのアクセスを許可してください。");
                alert.SetPositiveButton("OK", (sender, args) =>
                {
                    _Context.StartActivity(new Intent(Settings.ActionUsageAccessSettings));
                });
                alert.Show();
            }
            _Context.StartService(_intent);
        }

        public void StopService()
        {
            _Context.StopService(_intent);
        }

        /// <summary>
        /// 使用履歴へのPermissionが許可されているか確認する。
        /// </summary>
        /// <returns></returns>
        public bool isUsageStatsAllowed()
        {
            AppOpsManager appOps = (AppOpsManager)Forms.Context.GetSystemService(Context.AppOpsService);
            var mode = appOps.CheckOpNoThrow(AppOpsManager.OpstrGetUsageStats, Process.MyUid(), Forms.Context.PackageName);
            return (mode == AppOpsManagerMode.Allowed);
        }
    }
}



