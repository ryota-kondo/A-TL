using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATL.Helpers
{
    /// <summary>
    /// Iconとpackage取得のためのインジェクションクラス用のインターフェイス
    /// </summary>
    public interface IGetApplicationIconAndName
    {
        (string appName,string iconUrl) GetNameAndURL(string packageName);
    }
}
