using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ATL.Helpers
{
    /// <summary>
    /// DBへ保存したデータを一旦集計しやすい形で受け取るための構造体
    /// </summary>
    public struct AppNameAndExecTimeTemp
    {
        public string app_name { get; set; }
        public int exeTimeSecond { get; set; }
    }

    /// <summary>
    /// AppNameAndExecTimeTempのデータを画面表示へ適した形へ変換し表示するための構造体
    /// </summary>
    public struct AppNameAndExecTime
    {
        public string app_name { get; set; }
        public ImageSource iconImage { get; set; }
        public string exeTimeSecond { get; set; }
    }
}
