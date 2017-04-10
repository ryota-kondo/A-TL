using ATL.Helpers;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATL.Models
{
    /// <summary>
    /// 前ページに適用するModel
    /// </summary>
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

        /// <summary>
        /// IEnumerable&lt;AppNameAndExecTimeTemp&gt;を受け取り名前ごとの実行時間の合計を計算しIEnumerable AppNameAndExecTimeにて返す
        /// </summary>
        /// <param name="appExeTimeLists"></param>
        /// <returns></returns>
        private IEnumerable<AppNameAndExecTime> ConvartTimeList(IEnumerable<AppNameAndExecTimeTemp> appExeTimeLists)
        {
            var appNameGroup = appExeTimeLists.GroupBy(a => a.app_name);
            var appNameAndExecTime = new List<AppNameAndExecTime>();

            foreach (IGrouping<string, AppNameAndExecTimeTemp> v in appNameGroup)
            {
                AppNameAndExecTime t = new AppNameAndExecTime();

                // app名 & ICON
                var NameUrl = GetApplicationIconAndName.GetNameAndURL(v.Key);
                t.app_name = NameUrl.appName;
                t.icon_url = NameUrl.iconUrl;

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

                    t.exeTimeSecond = SubtractDateTime(startTime, endTime);

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

                    t.exeTimeSecond = SubtractDateTime(startTime, endTime);

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

                    t.exeTimeSecond = SubtractDateTime(startTime, endTime);

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

                    t.exeTimeSecond = SubtractDateTime(startTime, endTime);

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

                t.exeTimeSecond = SubtractDateTime(startTime, endTime);

                appExeTimeLists.Add(t);

            }
            return ConvartTimeList(appExeTimeLists);
        }

        public int DaysInMonth(DateTime dt)
        {
            return DateTime.DaysInMonth(dt.Year, dt.Month);
        }

        public int SubtractDateTime(DateTime Start, DateTime End)
        {
            var time = End - Start;
            return (int)time.TotalSeconds;

        }

    }
}
