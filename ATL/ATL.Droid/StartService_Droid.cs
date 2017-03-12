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

using ATL.Helpers;
using Xamarin.Forms;

namespace ATL.Droid
{
    class StartService_Droid : IStartService
    {
        public void StartService()
        {
            Toast.MakeText(Forms.Context,"StartService",ToastLength.Short).Show();
        }

        public void StopService()
        {
            Toast.MakeText(Forms.Context, "StopService", ToastLength.Short).Show();
        }
    }
}