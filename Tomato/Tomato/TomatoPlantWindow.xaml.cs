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
using System.Windows.Shapes;

namespace Tomato
{
    /// <summary>
    /// TomatoPlantWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TomatoPlantWindow : Window
    {
        public TomatoPlantWindow()
        {
            InitializeComponent();
        }

        private void giveupPlant_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                base.DragMove();
            }
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            contentCanvas.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            contentCanvas.Visibility = Visibility.Hidden;
        }
    }
}
