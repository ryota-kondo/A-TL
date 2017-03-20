using ATL.Helpers;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATL.Models
{
    class AllPageModel : BindableBase, IAllPageModel
    {
        public IStartService StatService { get; set; }
        public IConnectSqlite ConnectSqlite { get; set; }

        public AllPageModel(IStartService startService,IConnectSqlite connectSqlite)
        {
            this.StatService = startService;
            this.ConnectSqlite = connectSqlite;
        }

        public IEnumerable<AppNameAndExecTime> GetTodayLists()
        {
            var db = ConnectSqlite.GetItems();
            var appExeTimeLists = new List<AppNameAndExecTimeTemp>();


            //AppExeTimeListの形へ型変換しつつ代入
            foreach (var VARIABLE in db)
            {
                if (DateTime.Parse(VARIABLE.startTime) > DateTime.Today)
                {
                    AppNameAndExecTimeTemp t = new AppNameAndExecTimeTemp();

                    t.app_name = VARIABLE.app_name;
                    var startTime = DateTime.Parse(VARIABLE.startTime);
                    var endTime = DateTime.Parse(VARIABLE.endTime);

                    if (DateTime.Parse(VARIABLE.endTime) >= DateTime.Today.AddDays(1))
                    {
                        endTime = DateTime.Today.AddDays(1);
                    }

                    var a = endTime - startTime;
                    var b = a.Seconds;

                    t.exeTimeSecond = b;

                    appExeTimeLists.Add(t);
                }
            }

            // アプリ名ごとに合計時間を計測
            var temp = appExeTimeLists.GroupBy(a => a.app_name);
            var q = new List<AppNameAndExecTime>();

            foreach (IGrouping<string,AppNameAndExecTimeTemp> v in temp)
            {
                AppNameAndExecTime t = new AppNameAndExecTime();

                t.app_name = v.Key;

                var second = v.Sum(a => a.exeTimeSecond);
                var ts = new TimeSpan(0, 0, second);
                t.exeTimeSecond = ts.ToString();

                q.Add(t);
            }
            return q.OrderByDescending(a => a.exeTimeSecond);
        }
    }
}
