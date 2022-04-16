using Scene_Maker.Core;
using Scene_Maker.MVVM.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Scene_Maker.MVVM.View
{
    /// <summary>
    /// Interaction logic for RoutineCreatorView.xaml
    /// </summary>
    public partial class RoutineCreatorView : UserControl
    {
        public RoutineCreatorView()
        {
            InitializeComponent();
        }

        private void CreatorView_MouseMove(object sender, MouseEventArgs e)
        {
            var os = (FrameworkElement)e.OriginalSource;
            var vm = os.DataContext as DragHandler;
            if (vm != null)
                vm.HandleMouseMovement(sender, e);
        }

        private void CreatorView_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var os = (FrameworkElement) e.OriginalSource;
            var vm = os.DataContext as DragHandler;
            if (vm != null)
                vm.HandleMouseUp(sender, e);
        }

        private void CreatorView_MouseLeave(object sender, MouseEventArgs e)
        {
            var os = (FrameworkElement)e.OriginalSource;
            var vm = os.DataContext as DragHandler;
            if (vm != null)
                vm.HandleMouseEnter(sender, e);
        }
    }
}
