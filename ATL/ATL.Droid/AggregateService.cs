using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.App.Usage;

using Xamarin.Forms;
using System.Timers;
using ATL.Helpers;

namespace ATL.Droid
{
    /// <summary>
    /// �풓���A�v���̋N�����Ԃ��W�v����Service
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

        // private (int x, int y) value;


        private const int MIN_KEISOKU_SECOND = 1;

        // List<t_texecute_times> list2 = new List<t_texecute_times>();

        System.Timers.Timer timer = new System.Timers.Timer();
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
            //Toast.MakeText(Forms.Context, "�Ď��J�n", ToastLength.Short).Show();

            gStarted = true;

            PendingIntent pendingIntent;
            Notification navigate;
            var context = Forms.Context;

            Intent _intent = new Intent(context, typeof(MainActivity));
            pendingIntent = PendingIntent.GetActivity(context, 0, _intent, 0);

            navigate = new Notification.Builder(context)
                .SetContentTitle("A-TL")
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentText("�A�v���̎g�p�󋵂��v�����ł��B")
                .SetOngoing(true) //�풓������
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

            // �^�C�}�[���~
            timer.Stop();
        }

        public void OnElapsed_TimersTimer(object sender, ElapsedEventArgs e)
        {
            var _last_app_name = this.lastAppName;

            MyStruct eventInfo = kanshi();

            if(_last_app_name == eventInfo.app_name)
            {
                if(UsageEventType.MoveToForeground == eventInfo.event_name) // �v�����p��
                {
                    // this.ecxecTime += MIN_KEISOKU_SECOND;
                }
                else if(UsageEventType.MoveToBackground == eventInfo.event_name) // �v���I���
                {
                    InsertDb();

                    // this.ecxecTime = 0;
                    this.lastAppName = "";

                    appKeisokuFlag = false;
                }
            }
            else
            {
                // ���݌v�����ł����
                if(appKeisokuFlag)
                {
                    // this.ecxecTime += MIN_KEISOKU_SECOND;
                }

                if (UsageEventType.MoveToForeground == eventInfo.event_name) // �V���Ɍv���J�n
                {
                    InsertDb();

                    this.lastAppName = eventInfo.app_name;
                    // this.ecxecTime = 0;

                    appKeisokuFlag = true;
                    this.startTime = DateTime.Now.ToString();
                }
                else if (UsageEventType.MoveToBackground == eventInfo.event_name) // �O��̏I�����������ǎ����v���I���
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
            if (this.lastAppName == "") return;
            this.endTime = DateTime.Now.ToString();

            var temp = new t_texecute_times { app_name = this.lastAppName,  startTime = this.startTime, endTime = this.endTime };
            dbConnect.SaveItem(temp);

            this.startTime = "";
            this.endTime = "";
        }


        /// <summary>
        /// UsageEvents.Event()�ɋL�^���ꂽ�Ō�̃A�v�����Ƃ��̃C�x���g��Ԃ�
        /// </summary>
        /// <returns></returns>
        public MyStruct kanshi()
        {
            var _context = Forms.Context;

            var app_name = "none";
            var event_name = UsageEventType.None;

            //Calendar endCal;
            //endCal = Calendar.GetInstance(TimeZone.Default);
            //endCal.Set(Calendar.Date, DateTime.Now.Day);
            //endCal.Set(Calendar.Month, DateTime.Now.Month - 1);
            //endCal.Set(Calendar.Year, DateTime.Now.Year);

            var usageStatsManager = (UsageStatsManager)_context.GetSystemService(UsageStatsService);
            long time = GetUnixTime(DateTime.Now) * 1000;
            const long interval = 2 * 1000;
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
            MyStruct myStruct = new MyStruct(app_name, event_name);

            return myStruct;
        }

        public struct MyStruct
        {
            public string app_name;
            public UsageEventType event_name;

            public MyStruct(string appn,UsageEventType eventn)
            {
                app_name = appn;
                event_name = eventn;
            }

        }

        // UNIX�G�|�b�N��\��DateTime�I�u�W�F�N�g���擾
        private static DateTime UNIX_EPOCH =
            new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public static long GetUnixTime(DateTime targetTime)
        {
            // UTC���Ԃɕϊ�
            targetTime = targetTime.ToUniversalTime();

            // UNIX�G�|�b�N����̌o�ߎ��Ԃ��擾
            TimeSpan elapsedTime = targetTime - UNIX_EPOCH;

            // �o�ߕb���ɕϊ�
            return (long)elapsedTime.TotalSeconds;
        }
    }
}