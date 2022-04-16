using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Scene_Maker.Core
{
    internal interface DragHandler
    {
        void HandleMouseMovement(object sender, MouseEventArgs e);
        void HandleMouseDown(object sender, MouseEventArgs e);
        void HandleMouseUp(object sender, MouseEventArgs e);
        void HandleMouseLeave(object sender, MouseEventArgs e);
        void HandleMouseEnter(object sender, MouseEventArgs e);
    }
}
