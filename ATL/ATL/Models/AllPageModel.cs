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
        public ISaveAndLoad SaveAndLoad { get; set; }
        public IGetApplicationIconAndName GetApplicationIconAndName { get; set; }

        public AllPageModel(IStartService startService, IConnectSqlite connectSqlite, IGetApplicationIconAndName getApplicationIconAndName, ISaveAndLoad saveAndLoad)
        {
            this.StatService = startService;
            this.ConnectSqlite = connectSqlite;
            this.SaveAndLoad = saveAndLoad;
            this.GetApplicationIconAndName = getApplicationIconAndName;
        }


        private IEnumerable<AppNameAndExecTime> ConvartTimeList(IEnumerable<AppNameAndExecTimeTemp> appExeTimeLists)
        {
            // アプリ名ごとに合計時間を計測
            var appNameGroup = appExeTimeLists.GroupBy(a => a.app_name);
            var appNameAndExecTime = new List<AppNameAndExecTime>();

            foreach (IGrouping<string, AppNameAndExecTimeTemp> v in appNameGroup)
            {
                AppNameAndExecTime t = new AppNameAndExecTime();

                // app名 & ICON
                var NameUrl = GetApplicationIconAndName.GetNameAndURL(v.Key);
                t.app_name = NameUrl.Item1;
                t.icon_url = NameUrl.Item2;

                var second = v.Sum(a => a.exeTimeSecond);
                var ts = new TimeSpan(0, 0, second);
                t.exeTimeSecond = ts.ToString();

                appNameAndExecTime.Add(t);
            }
            return appNameAndExecTime.OrderByDescending(a => a.exeTimeSecond);
        }

        /// <summary>
        /// 当日のアプリ実行時間のリストをIEnumerable<AppNameAndExecTime>で返す
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AppNameAndExecTime> GetTodayLists()
        {
            var db = ConnectSqlite.GetItems();
            var appExeTimeLists = new List<AppNameAndExecTimeTemp>();

            // t_texecute_times をAppNameAndExecTimeTemp へ変換
            foreach (var tTexecuteTimes in db)
            {
                // 集計期間で絞込
                if (DateTime.Parse(tTexecuteTimes.startTime) >= DateTime.Today)
                {
                    AppNameAndExecTimeTemp t = new AppNameAndExecTimeTemp();

                    t.app_name = tTexecuteTimes.app_name;
                    var startTime = DateTime.Parse(tTexecuteTimes.startTime);
                    var endTime = DateTime.Parse(tTexecuteTimes.endTime);

                    if (DateTime.Parse(tTexecuteTimes.endTime) >= DateTime.Today.AddDays(1))
                    {
                        endTime = DateTime.Today.AddDays(1);
                    }

                    var a = endTime - startTime;
                    var b = a.Seconds;

                    t.exeTimeSecond = b;

                    appExeTimeLists.Add(t);
                }
            }
            return ConvartTimeList(appExeTimeLists);
        }

        /// <summary>
        /// 昨日のアプリ実行時間のリストをIEnumerable<AppNameAndExecTime>で返す
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AppNameAndExecTime> GetYesterdayLists()
        {
            var db = ConnectSqlite.GetItems();
            var appExeTimeLists = new List<AppNameAndExecTimeTemp>();

            // t_texecute_times をAppNameAndExecTimeTemp へ変換
            foreach (var tTexecuteTimes in db)
            {
                // 集計期間で絞込
                if (DateTime.Parse(tTexecuteTimes.startTime) > DateTime.Today.AddDays(-1) && DateTime.Parse(tTexecuteTimes.startTime) < DateTime.Today)
                {
                    AppNameAndExecTimeTemp t = new AppNameAndExecTimeTemp();

                    t.app_name = tTexecuteTimes.app_name;
                    var startTime = DateTime.Parse(tTexecuteTimes.startTime);
                    var endTime = DateTime.Parse(tTexecuteTimes.endTime);

                    if (DateTime.Parse(tTexecuteTimes.endTime) >= DateTime.Today)
                    {
                        endTime = DateTime.Today;
                    }

                    var a = endTime - startTime;
                    var b = a.Seconds;

                    t.exeTimeSecond = b;

                    appExeTimeLists.Add(t);
                }
            }
            return ConvartTimeList(appExeTimeLists);
        }

        /// <summary>
        /// 過去1週刊のアプリ実行時間のリストをIEnumerable<AppNameAndExecTime>で返す
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AppNameAndExecTime> GetWeekLists()
        {
            var db = ConnectSqlite.GetItems();
            var appExeTimeLists = new List<AppNameAndExecTimeTemp>();

            // t_texecute_times をAppNameAndExecTimeTemp へ変換
            foreach (var tTexecuteTimes in db)
            {
                // 集計期間で絞込
                if (DateTime.Parse(tTexecuteTimes.startTime) > DateTime.Today.AddDays(-7))
                {
                    AppNameAndExecTimeTemp t = new AppNameAndExecTimeTemp();

                    t.app_name = tTexecuteTimes.app_name;
                    var startTime = DateTime.Parse(tTexecuteTimes.startTime);
                    var endTime = DateTime.Parse(tTexecuteTimes.endTime);

                    endTime = DateTime.Today.AddDays(1);

                    var a = endTime - startTime;
                    var b = a.Seconds;

                    t.exeTimeSecond = b;

                    appExeTimeLists.Add(t);
                }
            }
            return ConvartTimeList(appExeTimeLists);
        }

        /// <summary>
        /// 今月のアプリ実行時間のリストをIEnumerable<AppNameAndExecTime>で返す
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AppNameAndExecTime> GetMonthLists()
        {
            var db = ConnectSqlite.GetItems();
            var appExeTimeLists = new List<AppNameAndExecTimeTemp>();

            // t_texecute_times をAppNameAndExecTimeTemp へ変換
            foreach (var tTexecuteTimes in db)
            {
                // 集計期間で絞込
                if (DateTime.Parse(tTexecuteTimes.startTime).Month == DateTime.Today.Month)
                {
                    AppNameAndExecTimeTemp t = new AppNameAndExecTimeTemp();

                    t.app_name = tTexecuteTimes.app_name;
                    var startTime = DateTime.Parse(tTexecuteTimes.startTime);
                    var endTime = DateTime.Parse(tTexecuteTimes.endTime);

                    if (DateTime.Parse(tTexecuteTimes.endTime).Month > DateTime.Today.Month)
                    {
                        endTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DaysInMonth(DateTime.Today));
                    }

                    var a = endTime - startTime;
                    var b = a.Seconds;

                    t.exeTimeSecond = b;

                    appExeTimeLists.Add(t);
                }
            }
            return ConvartTimeList(appExeTimeLists);
        }

        /// <summary>
        /// 集計したすべての期間のアプリ実行時間のリストをIEnumerable<AppNameAndExecTime>で返す
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AppNameAndExecTime> GetAlldataLists()
        {
            var db = ConnectSqlite.GetItems();
            var appExeTimeLists = new List<AppNameAndExecTimeTemp>();

            // t_texecute_times をAppNameAndExecTimeTemp へ変換
            foreach (var tTexecuteTimes in db)
            {
                // 集計期間で絞込

                AppNameAndExecTimeTemp t = new AppNameAndExecTimeTemp();

                t.app_name = tTexecuteTimes.app_name;
                var startTime = DateTime.Parse(tTexecuteTimes.startTime);
                var endTime = DateTime.Parse(tTexecuteTimes.endTime);


                endTime = DateTime.Today.AddDays(1);


                var a = endTime - startTime;
                var b = a.Seconds;

                t.exeTimeSecond = b;

                appExeTimeLists.Add(t);

            }
            return ConvartTimeList(appExeTimeLists);
        }

        public static int DaysInMonth(DateTime dt)
        {
            return DateTime.DaysInMonth(dt.Year, dt.Month);
        }
    }
}
