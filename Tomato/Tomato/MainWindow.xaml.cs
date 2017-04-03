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

        private void goalsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateGoal.IsEnabled = goalsList.SelectedIndex >= 0;

            if (goalsList.SelectedIndex < 0)
            {
                return;
            }
            var item = goalsList.SelectedItem as ListBoxItem;
            if (item == null)
            {
                throw new Exception("goalsList_SelectionChanged failed, item is nil.");
            }
            var seed = item.Content as TomatoSeed;
            if (seed == null)
            {
                throw new Exception("goalsList_SelectionChanged failed, seed is nil.");
            }
            titleEdit.Text = seed.Title;
            detailEdit.Text = seed.Detail;
            countEdit.Value = seed.ExpectTomatoCount;
            priorityEdit.SelectedIndex = (int)seed.Priority;
        }

        private void updateGoal_Click(object sender, RoutedEventArgs e)
        {
            if (goalsList.SelectedIndex < 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(titleEdit.Text) ||
                string.IsNullOrEmpty(detailEdit.Text))
            {
                MessageBox.Show(Properties.Resources.EditorCreateContentEmptyContent, Properties.Resources.EditorCreateContentEmptyCaption,
                     MessageBoxButton.OK);
                return;
            }
            var item = goalsList.SelectedItem as ListBoxItem;
            if (item == null)
            {
                throw new Exception("updateGoal_Click failed, item is nil.");
            }
            var seed = item.Content as TomatoSeed;
            if (seed == null)
            {
                throw new Exception("updateGoal_Click failed, seed is nil.");
            }
            seed.Title = titleEdit.Text; ;
            seed.Detail = detailEdit.Text;
            seed.ExpectTomatoCount = (int)countEdit.Value;
            seed.Priority = (TOMATO_PRI)priorityEdit.SelectedIndex;
        }

        private void addGoal_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(titleEdit.Text) ||
                string.IsNullOrEmpty(detailEdit.Text))
            {
                MessageBox.Show(Properties.Resources.EditorCreateContentEmptyContent, Properties.Resources.EditorCreateContentEmptyCaption,
                     MessageBoxButton.OK);
                return;
            }
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
            var result = MessageBox.Show(Properties.Resources.MenuDeleteTipContent, Properties.Resources.MenuDeleteTipCaption,
                 MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                goalsList.Items.RemoveAt(goalsList.SelectedIndex);
            }
        }

        private void MenuTodayPlanItem_Click(object sender, RoutedEventArgs e)
        {
            if (goalsList.SelectedIndex < 0)
            {
                return;
            }
            var item = goalsList.SelectedItem as ListBoxItem;
            goalsList.Items.RemoveAt(goalsList.SelectedIndex);
            planList.Items.Add(item);
        }

        private void MenuGowItem_Click(object sender, RoutedEventArgs e)
        {
            if (planList.SelectedIndex < 0)
            {
                return;
            }
            var item = planList.SelectedItem as ListBoxItem;
            if (item == null)
            {
                throw new Exception("MenuGowItem_Click failed, item is nil.");
            }
            var seed = item.Content as TomatoSeed;
            if (seed == null)
            {
                throw new Exception("MenuGowItem_Click failed, seed is nil.");
            }
            var plantWindow = new TomatoPlantWindow();
            seed.Sow();
            plantWindow.SetTomatoPlant(seed.AllPlants[seed.CurGrowPlantIdx]);
            plantWindow.Show();
        }

        private void MenuGiveupItem_Click(object sender, RoutedEventArgs e)
        {
            if (planList.SelectedIndex < 0)
            {
                return;
            }
            
            var result = MessageBox.Show(Properties.Resources.MenuDeleteTipContent, Properties.Resources.MenuDeleteTipCaption,
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                planList.Items.RemoveAt(planList.SelectedIndex);
            }
        }

        private void MenuLeaveTodayItem_Click(object sender, RoutedEventArgs e)
        {
            if (planList.SelectedIndex < 0)
            {
                return;
            }
            var item = planList.SelectedItem as ListBoxItem;
            planList.Items.RemoveAt(planList.SelectedIndex);
            goalsList.Items.Add(item);
        }
    }
}
