using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;
using ATL.Helpers;

namespace ATL.Droid
{
    public class ConnectSqlite_Dorid: IConnectSqlite
    {
        private static readonly object Locker = new object();
        SQLiteConnection _db;

        public ConnectSqlite_Dorid()
        {
            _db = GetConnection();
            _db.CreateTable<t_texecute_times>();
        }

        public SQLiteConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, "execute_time_data.sqlite");
            return new SQLiteConnection(path);
        }

        public IEnumerable<t_texecute_times> GetItems()
        {
            lock (Locker)
            {
                return _db.Table<t_texecute_times>();
            }
        }

        public int SaveItem(t_texecute_times item)
        {
            lock (Locker)
            {
                if (item.id != 0)
                {
                    _db.Update(item);
                    return item.id;
                }
                return _db.Insert(item);
            }
        }

        public int DeleteItem(t_texecute_times item)
        {
            lock (Locker)
            {
                _db.Delete(item);
                return item.id;
            }
        }
    }
}