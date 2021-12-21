using Scene_Maker.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene_Maker.MVVM.ViewModel
{
    class SimpleNavOptionsViewModel
    {
        public RelayCommand BackCommand { get; set; }

        public SimpleNavOptionsViewModel(RelayCommand backCommand = null)
        {
            BackCommand = backCommand;
        }
    }
}
