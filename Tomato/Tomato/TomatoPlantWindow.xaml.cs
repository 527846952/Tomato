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
        private TomatoPlant curPlant;
        private SolidColorBrush growingTimeColor = new SolidColorBrush(Color.FromArgb(255, 255, 180, 0));
        private SolidColorBrush restingTimeColor = new SolidColorBrush(Color.FromArgb(255, 0, 180, 0));

        public TomatoPlantWindow()
        {
            InitializeComponent();
        }

        public void SetTomatoPlant(TomatoPlant plant)
        {
            if (curPlant != null && curPlant.State != TOMATO_PLANT_STATE.Finish &&
                curPlant.State != TOMATO_PLANT_STATE.Giveup)
            {
                throw new Exception("SetToMatoPlant fail, curPlant.State is " + curPlant.State);
            }
            curPlant = plant;

            plantTitle.Content = curPlant.Seed.Title;

            curPlant.OnGrowingRateChange += CurPlant_OnGrowingRateChange;
            curPlant.OnReapFruit += CurPlant_OnReapFruit;
            curPlant.OnRestRateChange += CurPlant_OnRestRateChange;
            curPlant.OnFinish += CurPlant_OnFinish;
        }

        private void QueryStartRest()
        {
            if (curPlant == null)
            {
                throw new Exception("QueryStartRest fail, curPlant is nil.");
            }
            var result = MessageBox.Show(Properties.Resources.QueryStartRestTipContent, Properties.Resources.QueryStartRestTipCaption,
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                curPlant.StartRest();
            }
        }
        
        private void CurPlant_OnGrowingRateChange(double rate)
        {
            if (curPlant == null)
            {
                throw new Exception("CurPlant_OnGrowingRateChange fail, curPlant is nil.");
            }

            remainTime.Foreground = growingTimeColor;
            remainTime.Content = (curPlant.RemainLifeSeconds / 60) + ":" + (curPlant.RemainLifeSeconds % 60);
        }

        private void CurPlant_OnReapFruit(TomatoFruit fruit)
        {
            if (curPlant == null)
            {
                throw new Exception("CurPlant_OnReapFruit fail, curPlant is nil.");
            }
            QueryStartRest();
        }

        private void CurPlant_OnRestRateChange(double rate)
        {
            if (curPlant == null)
            {
                throw new Exception("CurPlant_OnRestRateChange fail, curPlant is nil.");
            }
            remainTime.Foreground = restingTimeColor;
            remainTime.Content = (curPlant.RemainRestSeconds / 60) + ":" + (curPlant.RemainRestSeconds % 60);
        }

        private void CurPlant_OnFinish(TomatoPlant plant)
        {
            if (curPlant == null)
            {
                throw new Exception("CurPlant_OnFinish fail, curPlant is nil.");
            }
        }

        private void pausePlant_Click(object sender, RoutedEventArgs e)
        {
            if (curPlant == null)
            {
                throw new Exception("pausePlant_Click fail, curPlant is nil.");
            }
            curPlant.Pause("no data");

            pausePlant.Visibility = Visibility.Hidden;
            replayPlant.Visibility = Visibility.Visible;
        }

        private void replayPlant_Click(object sender, RoutedEventArgs e)
        {
            if (curPlant == null)
            {
                throw new Exception("pausePlant_Click fail, curPlant is nil.");
            }
            curPlant.Replay();

            replayPlant.Visibility = Visibility.Hidden;
            pausePlant.Visibility = Visibility.Visible;
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
            contentBorder.Visibility = Visibility.Visible;
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            contentBorder.Visibility = Visibility.Hidden;
        }

        
    }
}
