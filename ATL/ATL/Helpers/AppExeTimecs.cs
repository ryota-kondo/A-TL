using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATL.Helpers
{
    public struct AppNameAndExecTimeTemp
    {
        public string app_name { get; set; }
        public int exeTimeSecond { get; set; }
    }

    public struct AppNameAndExecTime
    {
        public string app_name { get; set; }
        public string icon_url { get; set; }
        public String exeTimeSecond { get; set; }
    }
}
