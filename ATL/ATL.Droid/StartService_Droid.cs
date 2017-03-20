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
    /// Service�̏풓��ON/OFF�̋@�\���C���W�F�N�V���������N���X
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
                alert.SetMessage("�v�����邽�߂ɂ͎g�p�����ւ̃A�N�Z�X�������Ă��������B");
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
        /// �g�p�����ւ�Permission���������Ă��邩�m�F�����B
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



