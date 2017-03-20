using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATL.Helpers
{
    /// <summary>
    /// 各プラットフォームごとのJsonデータの保存のためのインターフェイス
    /// </summary>
    public interface ISaveAndLoad
    {
        void SaveData(string filename, string text);

        string LoadData(string filename);
    }
}
