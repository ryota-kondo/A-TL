using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATL.Helpers
{
    public interface IAllPageModel : INotifyPropertyChanged
    {
        IStartService StatService { get; set; }
    }
}
