using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using ATL.Helpers;

namespace ATL.ViewModels
{
    public class MenuPageViewModel : BindableBase
    {

        private readonly IAllPageModel _model;

        public DelegateCommand DeleteCommand { get; set; }
        public MenuPageViewModel(IAllPageModel model)
        {
            _model = model;
            DeleteCommand = new DelegateCommand(ResetDB);
        }

        

        private void ResetDB()
        {
            _model.ConnectSqlite.DeleteItem();
        }
    }
}
