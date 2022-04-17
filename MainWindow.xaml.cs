using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scene_Maker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SizeToContent = System.Windows.SizeToContent.Manual;
        }

        private void windowBorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;
            this.DragMove();
        }

        private void closeButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void minimizeButtonClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void maximizeButtonClick(object sender, RoutedEventArgs e)

        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized; 
        }
    }
}
