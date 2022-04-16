using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene_Maker.MVVM.ViewModel.Routines.RoutineCreator
{
    class WaveFormData
    {
        public class SeekerData
        {
            public SeekerData()
            {

            }
            public int BarWidth { get; set; }
            public int BarStart { get; set; }
            public int BarEnd { get; set; }
        }

        public SeekerData SeekerViewData { get; set; }
    }
}
