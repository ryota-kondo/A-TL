﻿using ATL.Helpers;
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
        public ISaveAndLoad SaveAndLoad { get; set; }
        public IGetApplicationIconAndName GetApplicationIconAndName { get; set; }

        public AllPageModel(IStartService startService,IConnectSqlite connectSqlite,IGetApplicationIconAndName getApplicationIconAndName,ISaveAndLoad saveAndLoad)
        {
            this.StatService = startService;
            this.ConnectSqlite = connectSqlite;
            this.SaveAndLoad = saveAndLoad;
            this.GetApplicationIconAndName = getApplicationIconAndName;
        }

        /// <summary>
        /// 当日のアプリ実行時間のリストをIEnumerable<AppNameAndExecTime>で返す
        /// </summary>
        /// <returns></returns>
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

            foreach (IGrouping<string, AppNameAndExecTimeTemp> v in temp)
            {
                AppNameAndExecTime t = new AppNameAndExecTime();

                // app名 & ICON
                var NameUrl = GetApplicationIconAndName.GetNameAndURL(v.Key);
                t.app_name = NameUrl.Item1;
                t.icon_url = NameUrl.Item2;

                var second = v.Sum(a => a.exeTimeSecond);
                var ts = new TimeSpan(0, 0, second);
                t.exeTimeSecond = ts.ToString();

                q.Add(t);
            }
            return q.OrderByDescending(a => a.exeTimeSecond);
        }
    }
}
