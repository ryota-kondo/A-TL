using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
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
    /// <summary>
    /// パッケージ名からアプリ名とアイコンを取得
    /// </summary>
    class GetApplicationIconAndName_Droid: IGetApplicationIconAndName
    {
        public NameAndUrl GetNameAndURL(string packageName)
        {

            PackageManager packageManager = Forms.Context.PackageManager;
            var appList = packageManager.GetInstalledApplications(PackageInfoFlags.Activities);

            NameAndUrl appNameAndIcon;
            appNameAndIcon.appName = appList.Where(b => b.PackageName == packageName).Select(a => a.LoadLabel(packageManager)).First();

            var applicationIcon = packageManager.GetApplicationIcon(packageName);

            //----------------
            var bitmap = drawableToBitmap(applicationIcon);

            var memoryStream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, memoryStream);
            appNameAndIcon.iconUrl = memoryStream.ToArray();
            //----------

            return appNameAndIcon;
        }

        public static Bitmap drawableToBitmap(Drawable drawable)
        {
            Bitmap bitmap = null;

            if (drawable is BitmapDrawable) {
                BitmapDrawable bitmapDrawable = (BitmapDrawable)drawable;
                if (bitmapDrawable.Bitmap != null)
                {
                    return bitmapDrawable.Bitmap;
                }
            }

            if (drawable.IntrinsicWidth <= 0 || drawable.IntrinsicHeight <= 0)
            {
                bitmap = Bitmap.CreateBitmap(1, 1, Bitmap.Config.Argb8888);
            }
            else
            {
                bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888);
            }

            Canvas canvas = new Canvas(bitmap);
            drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
            drawable.Draw(canvas);
            return bitmap;
        }


    }
}


