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

        public string GetDbString()
        {
            var a =ConnectSqlite.GetItems();
            var str = "";
            foreach(var list in a)
            {
                str += $"{list.id}:{list.app_name} : \n\r {list.startTime} \n\r to \n\r{list.endTime}\n\r\n\r";
            }
            return str;
        }
    }
}
