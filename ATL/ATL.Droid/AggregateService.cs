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
using Android.App.Usage;

using Xamarin.Forms;
using static Android.App.ActivityManager;

using Android.Icu.Util;
using TimeZone = Android.Icu.Util.TimeZone;
using System.Timers;
using System.Threading;
using System.IO;
using SQLite;
using ATL.Helpers;

namespace ATL.Droid
{
    /// <summary>
    /// 常駐しアプリの起動時間を集計するService
    /// </summary>
    [Service]
    class AggregateService : Service
    {
        public override IBinder OnBind(Intent intent) => null;

        private bool gStarted = false;
        private bool appKeisokuFlag = false;
        private bool IsStarted() => gStarted;
        private string lastAppName = "";
        // private int ecxecTime;
        string startTime, endTime;

        private (int x, int y) value;


        private const int MIN_KEISOKU_SECOND = 1;

        // List<t_texecute_times> list2 = new List<t_texecute_times>();

        System.Timers.Timer timer = new System.Timers.Timer();

        private static readonly object Locker = new object();

        private ConnectSqlite_Dorid dbConnect;

        public override void OnCreate()
        {
            // Toast.MakeText(this, "onCreate", ToastLength.Short).Show();
            dbConnect = new ConnectSqlite_Dorid();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if(IsStarted())
            {
                return StartCommandResult.NotSticky;
            }
            Toast.MakeText(Forms.Context, "監視開始", ToastLength.Short).Show();

            gStarted = true;

            PendingIntent pendingIntent;
            Notification navigate;
            var context = Forms.Context;

            Intent _intent = new Intent(context, typeof(AggregateService));
            pendingIntent = PendingIntent.GetService(context, 0, _intent, 0);

            navigate = new Notification.Builder(context)
                .SetContentTitle("A-TL")
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentText("アプリの使用状況を計測中です。")
                .SetOngoing(true) //常駐させる
                .SetContentIntent(pendingIntent)
                .Build();

            StartForeground(startId, navigate);

            //
            timer.Elapsed += new ElapsedEventHandler(OnElapsed_TimersTimer);
            timer.Interval = MIN_KEISOKU_SECOND *1000;
            timer.Start();

            return StartCommandResult.NotSticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            gStarted = false;

            // タイマーを停止
            timer.Stop();

            //var _toastString = "";
            //foreach(var l in list2)
            //{
            //    _toastString += $"{l.app_name} : \n\r {l.startTime} to {l.endTime}\n\r";
            //}


            // Toast.MakeText(Forms.Context, _toastString, ToastLength.Long).Show();
        }

        public void OnElapsed_TimersTimer(object sender, ElapsedEventArgs e)
        {
            var _last_app_name = this.lastAppName;

            (string _app_name,UsageEventType _event_name) eventInfo = kanshi();

            if(_last_app_name == eventInfo._app_name)
            {
                if(UsageEventType.MoveToForeground == eventInfo._event_name) // 計測を継続
                {
                    // this.ecxecTime += MIN_KEISOKU_SECOND;
                }
                else if(UsageEventType.MoveToBackground == eventInfo._event_name) // 計測終わり
                {
                    InsertDb();

                    // this.ecxecTime = 0;
                    this.lastAppName = "";

                    appKeisokuFlag = false;
                }
            }
            else
            {
                // 現在計測中であれば
                if(appKeisokuFlag)
                {
                    // this.ecxecTime += MIN_KEISOKU_SECOND;
                }

                if (UsageEventType.MoveToForeground == eventInfo._event_name) // 新たに計測開始
                {
                    InsertDb();

                    this.lastAppName = eventInfo._app_name;
                    // this.ecxecTime = 0;

                    appKeisokuFlag = true;
                    this.startTime = DateTime.Now.ToString();
                }
                else if (UsageEventType.MoveToBackground == eventInfo._event_name) // 前回の終了が無いけど実質計測終わり
                {
                    InsertDb();

                    // this.ecxecTime = 0;
                    this.lastAppName = "";

                    appKeisokuFlag = false;
                }
            }
        }

        private void InsertDb()
        {
            if(!(this.lastAppName == ""))
            {
                this.endTime = DateTime.Now.ToString();

                var temp = new t_texecute_times { app_name = this.lastAppName,  startTime = this.startTime, endTime = this.endTime };
                // list2.Add(temp);
                dbConnect.SaveItem(temp);

                this.startTime = "";
                this.endTime = "";
            }
        }


        /// <summary>
        /// UsageEvents.Event()に記録された最後のアプリ名とそのイベントを返す
        /// </summary>
        /// <returns></returns>
        public (string,UsageEventType) kanshi()
        {
            var _context = Forms.Context;

            var app_name = "none";
            var event_name = UsageEventType.None;

            UsageStatsManager usageStatsManager;

            Calendar endCal;
            endCal = Calendar.GetInstance(TimeZone.Default);
            endCal.Set(Calendar.Date, DateTime.Now.Day);
            endCal.Set(Calendar.Month, DateTime.Now.Month - 1);
            endCal.Set(Calendar.Year, DateTime.Now.Year);

            var currentTimeEnd = endCal.TimeInMillis;

            usageStatsManager = (UsageStatsManager)_context.GetSystemService(UsageStatsService);
            long time = endCal.TimeInMillis;
            long interval = 2 * 1000;
            UsageEvents events = usageStatsManager.QueryEvents(time - interval, time);
            while (events.HasNextEvent)
            {
                var event1 = new UsageEvents.Event();
                if (events.GetNextEvent(event1) ||( event1.EventType == UsageEventType.MoveToForeground && event1.EventType == UsageEventType.MoveToBackground) )
                {
                    app_name = event1.PackageName;
                    event_name = event1.EventType;
                }
            }
            return (app_name,event_name);
        }

        
    }
}