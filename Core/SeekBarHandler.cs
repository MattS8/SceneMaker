using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scene_Maker.Core
{
    internal interface SeekBarHandler
    {
        void OnSeekBarMoved(float fromStart, float fromEnd);
    }
}
