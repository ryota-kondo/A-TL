using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATL.Helpers
{
    /// <summary>
    /// ローカルデータベースへ計測したアプリ実行時間を保存するクラスのインジェクションのためのインターフェイス
    /// </summary>
    public interface IConnectSqlite
    {
        SQLiteConnection GetConnection();

        IEnumerable<t_texecute_times> GetItems();

        int SaveItem(t_texecute_times item);

        void DeleteItem();
    }
}
