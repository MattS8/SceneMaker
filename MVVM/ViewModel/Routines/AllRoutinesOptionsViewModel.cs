using Scene_Maker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene_Maker.MVVM.ViewModel
{
    class AllRoutinesOptionsViewModel
    {
        public RelayCommand NewRoutineCommand { get; set; }

        private MainViewModel Parent { get; set; }

        public AllRoutinesOptionsViewModel(MainViewModel parent)
        {
            Parent = parent;

            NewRoutineCommand = new RelayCommand(o =>
            {
                Parent.NewRoutineCommand.Execute(o);
            });
        }

    }
}
