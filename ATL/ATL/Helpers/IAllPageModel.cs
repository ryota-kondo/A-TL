using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATL.Helpers
{
    /// <summary>
    /// 各Modelのインジェクション用のコンテナインターフェイス
    /// </summary>
    public interface IAllPageModel : INotifyPropertyChanged
    {
        IStartService StatService { get; set; }
        IConnectSqlite ConnectSqlite { get; set; }
        ISaveAndLoad SaveAndLoad { get; set; }

        IEnumerable<AppNameAndExecTime> GetTodayLists();
        IEnumerable<AppNameAndExecTime> GetYesterdayLists();
        IEnumerable<AppNameAndExecTime> GetWeekLists();
        IEnumerable<AppNameAndExecTime> GetMonthLists();
        IEnumerable<AppNameAndExecTime> GetAlldataLists();
    }
}
