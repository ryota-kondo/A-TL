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

        public IEnumerable<AppExeTimeList> GetTodayLists()
        {
            var db = ConnectSqlite.GetItems();
            var appExeTimeLists = new List<AppExeTimeList>();


            //AppExeTimeListの形へ型変換しつつ代入
            foreach (var VARIABLE in db)
            {
                if (DateTime.Parse(VARIABLE.startTime) > DateTime.Today)
                {
                    AppExeTimeList t = new AppExeTimeList();

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
            var q = new List<AppExeTimeList>();

            foreach (IGrouping<string,AppExeTimeList> v in temp)
            {
                AppExeTimeList t = new AppExeTimeList();

                t.app_name = v.Key;
                t.exeTimeSecond = v.Sum(a => a.exeTimeSecond);
                q.Add(t);
            }
            return q.OrderByDescending(a => a.exeTimeSecond);
        }
    }
}
