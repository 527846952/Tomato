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
using System.Windows.Threading;

namespace Tomato
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timerDriver;
        private TomatoMgr tomatoMgr;

        public MainWindow()
        {
            InitializeComponent();

            InitTomotoMgr();
            InitTimer();
        }

        private void InitTomotoMgr()
        {
            tomatoMgr = new TomatoMgr();
        }

        private void InitTimer()
        {
            timerDriver = new DispatcherTimer();
            timerDriver.Interval = TimeSpan.FromMilliseconds(1000);
            timerDriver.Tick += TimerDriver_Tick;
            timerDriver.Start();
        }

        private void TimerDriver_Tick(object sender, EventArgs e)
        {
            TomatoMgr.TimeLoseSecond();
        }

        private void addGoal_Click(object sender, RoutedEventArgs e)
        {
            var seed = new TomatoSeed();
            seed.Title = titleEdit.Text; ;
            seed.Detail = detailEdit.Text;
            seed.ExpectTomatoCount = (int)countEdit.Value;
            seed.Priority = (TOMATO_PRI)priorityEdit.SelectedIndex;

            ListBoxItem boxItem = new ListBoxItem();
            boxItem.Content = seed;
            goalsList.Items.Add(boxItem);
        }

        private void countEdit_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (labTargetCount == null)
            {
                return;
            }
            labTargetCount.Content = (int)countEdit.Value;
        }

        private void MenuDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (goalsList.SelectedIndex < 0)
            {
                return;
            }
            goalsList.Items.RemoveAt(goalsList.SelectedIndex);
        }

        private void MenuTodayPlanItem_Click(object sender, RoutedEventArgs e)
        {
            if (goalsList.SelectedIndex < 0)
            {
                return;
            }
            var seed = goalsList.SelectedItem as TomatoSeed;
            var plantWindow = new TomatoPlantWindow();
            seed.Sow();
            plantWindow.SetTomatoPlant(seed.AllPlants[seed.CurGrowPlantIdx]);
            plantWindow.Show();
        }
    }
}
