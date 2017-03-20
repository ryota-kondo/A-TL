using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Test.Mock;
using Android.Views;
using Android.Widget;
using ATL.Helpers;
using Xamarin.Forms;

namespace ATL.Droid
{
    class GetApplicationIconAndName_Droid: IGetApplicationIconAndName
    {
        public (string appName, string iconUrl) GetNameAndURL(string packageName)
        {

            PackageManager packageManager = Forms.Context.PackageManager;
            var appList = packageManager.GetInstalledApplications(PackageInfoFlags.Activities);

            (string appName, string iconUrl) appNameAndIcon;
            appNameAndIcon.appName = appList.Where(b => b.PackageName == packageName).Select(a => a.LoadLabel(packageManager)).First();


            var dr = packageManager.GetApplicationIcon(packageName);
            // var bm = ((BitmapDrawable) dr).Bitmap;





            appNameAndIcon.iconUrl = dr.ToString();
            return appNameAndIcon;
        }
    }
}