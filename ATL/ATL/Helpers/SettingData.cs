using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATL.Helpers
{
    /// <summary>
    /// 設定データ用の構造体
    /// </summary>
    public struct SettingData
    {
        public bool Startup { get; set; }

        public SettingData(bool b)
        {
            Startup = b;
        }
    }
}
