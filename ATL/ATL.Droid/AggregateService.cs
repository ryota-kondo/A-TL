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
using Xamarin.Forms;

namespace ATL.Droid
{
    /// <summary>
    /// �풓���A�v���̋N�����Ԃ��W�v����Service
    /// </summary>
    [Service]
    class AggregateService : Service
    {
        public override IBinder OnBind(Intent intent) => throw new NotImplementedException();

        private bool gStarted = false;
        public bool IsStarted() => gStarted;

        public override void OnCreate()
        {
            Toast.MakeText(this, "onCreate", ToastLength.Short).Show();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if(IsStarted())
            {
                return StartCommandResult.NotSticky;
            }

            Toast.MakeText(Forms.Context, "OnStartCommand", ToastLength.Short).Show();

            gStarted = true;

            PendingIntent pendingIntent;
            Notification navigate;
            var context = Forms.Context;

            Intent _intent = new Intent(context, typeof(AggregateService));
            pendingIntent = PendingIntent.GetService(context, 0, _intent, 0);

            navigate = new Notification.Builder(context)
                .SetContentTitle("A-TL")
                .SetSmallIcon(Resource.Drawable.icon)
                .SetContentText("�A�v���̎g�p�󋵂��v�����ł��B")
                .SetOngoing(true) //�풓������
                .SetContentIntent(pendingIntent)
                .Build();

            StartForeground(startId, navigate);

            return StartCommandResult.NotSticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            gStarted = false;

            Toast.MakeText(Forms.Context, "OnDestroy", ToastLength.Short).Show();
        }
        
    }
}