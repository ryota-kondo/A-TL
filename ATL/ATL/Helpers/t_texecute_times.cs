using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATL.Helpers
{
    /// <summary>
    /// 集計したアプリ実行時間を保存するDB用の構造体
    /// </summary>
    public struct t_texecute_times
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string app_name { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
    }
}
