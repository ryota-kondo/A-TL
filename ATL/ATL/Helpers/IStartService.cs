﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATL.Helpers
{
    /// <summary>
    /// 各プラットフォームで監視を常駐させるためのインターフェイス
    /// </summary>
    public interface IStartService
    {
        void StartService();
        void StopService();
    }
}
