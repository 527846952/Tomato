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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Tomato
{
    /// <summary>
    /// TomatoPlantWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TomatoPlantWindow : Window
    {
        private readonly string TOMATON_CHAR = "♥";
        private readonly string TOMATON_SPACE_CHAR = "♡";
        private TomatoPlant curPlant;
        private SolidColorBrush growingTimeColor = new SolidColorBrush(Color.FromArgb(255, 255, 180, 0));
        private SolidColorBrush restingTimeColor = new SolidColorBrush(Color.FromArgb(255, 0, 180, 0));
        private Storyboard showBgStory;
        private Storyboard hideBgStory;

        public TomatoPlantWindow()
        {
            InitializeComponent();

            showBgStory = FindResource("showBackground") as Storyboard;
            hideBgStory = FindResource("hideBackground") as Storyboard;
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

            RefreshShowGrowingTime();
            RefreshShowTomatoCount();

            curPlant.OnGrowingRateChange += CurPlant_OnGrowingRateChange;
            curPlant.OnReapFruit += CurPlant_OnReapFruit;
            curPlant.OnRestRateChange += CurPlant_OnRestRateChange;
            curPlant.OnFinish += CurPlant_OnFinish;
        }

        private void RemoveTomatoPlant(TomatoPlant plant)
        {
            plant.OnGrowingRateChange -= CurPlant_OnGrowingRateChange;
            plant.OnReapFruit -= CurPlant_OnReapFruit;
            plant.OnRestRateChange -= CurPlant_OnRestRateChange;
            plant.OnFinish -= CurPlant_OnFinish;
        }

        private void RefreshShowGrowingTime()
        {
            remainTime.Foreground = growingTimeColor;
            remainTime.Content = string.Format("{0:D2}:{1:D2}", curPlant.RemainLifeSeconds / 60, curPlant.RemainLifeSeconds % 60);
        }

        private void RefreshShowRestTime()
        {
            remainTime.Foreground = restingTimeColor;
            remainTime.Content = string.Format("{0:D2}:{1:D2}", curPlant.RemainRestSeconds / 60, curPlant.RemainRestSeconds % 60);
        }

        private void SwitchDoingView()
        {
            replayPlant.Visibility = Visibility.Hidden;
            pausePlant.Visibility = Visibility.Visible;
        }

        private void SwitchPauseView()
        {
            pausePlant.Visibility = Visibility.Hidden;
            replayPlant.Visibility = Visibility.Visible;
        }

        private void SwitchRestView()
        {
            pausePlant.Visibility = Visibility.Hidden;
            replayPlant.Visibility = Visibility.Hidden;
        }

        private void RefreshShowTomatoCount()
        {
            if (curPlant == null)
            {
                throw new Exception("RefreshShowTomatoCount fail, curPlant is nil.");
            }
            var sumCount = curPlant.Seed.SumTomatoCount;
            if (sumCount > TomatoSeed.MAX_TOMATO_PLANT_COUNT)
            {
                throw new Exception("RefreshShowTomatoCount failed, sumCount greater than TomatoSeed.MAX_TOMATO_PLANT_COUNT " 
                    + TomatoSeed.MAX_TOMATO_PLANT_COUNT);
            }
            var remainCount = curPlant.Seed.RemainTomatoCount;
            int tCountA = sumCount > 8 ? 8 : sumCount % 8;
            int tCountB = sumCount < 8 ? 0 : sumCount % 8;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < tCountA; i++)
            {
                if (i > remainCount - 1)
                {
                    stringBuilder.Append(TOMATON_SPACE_CHAR);
                }
                else
                {
                    stringBuilder.Append(TOMATON_CHAR);
                }
            }
            tomatoCountA.Text = stringBuilder.ToString();

            stringBuilder.Clear();
            for (int i = 0; i < tCountB; i++)
            {
                if (i > remainCount - 1 - tCountA)
                {
                    stringBuilder.Append(TOMATON_SPACE_CHAR);
                }
                else
                {
                    stringBuilder.Append(TOMATON_CHAR);
                }
            }
            tomatoCountB.Text = stringBuilder.ToString();
        }

        private void QueryStartRest()
        {
            if (curPlant == null)
            {
                throw new Exception("QueryStartRest fail, curPlant is nil.");
            }
            var result = MessageBox.Show(this, Properties.Resources.QueryStartRestTipContent, Properties.Resources.QueryStartRestTipCaption,
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                SwitchRestView();
                curPlant.StartRest();
            }
        }
        
        private void CurPlant_OnGrowingRateChange(double rate)
        {
            if (curPlant == null)
            {
                throw new Exception("CurPlant_OnGrowingRateChange fail, curPlant is nil.");
            }

            RefreshShowGrowingTime();
        }

        private void CurPlant_OnReapFruit(TomatoFruit fruit)
        {
            if (curPlant == null)
            {
                throw new Exception("CurPlant_OnReapFruit fail, curPlant is nil.");
            }

            RefreshShowRestTime();
            SwitchPauseView();

            QueryStartRest();
        }

        private void CurPlant_OnRestRateChange(double rate)
        {
            if (curPlant == null)
            {
                throw new Exception("CurPlant_OnRestRateChange fail, curPlant is nil.");
            }
            RefreshShowRestTime();
        }

        private void CurPlant_OnFinish(TomatoPlant plant)
        {
            if (curPlant != plant)
            {
                throw new Exception("CurPlant_OnFinish fail, curPlant not match finishPlant.");
            }
            RemoveTomatoPlant(plant);

            if (plant.Seed.RemainTomatoCount > 0)
            {
                var selectPlant = plant.Seed.SelectNextPlant();
                SetTomatoPlant(selectPlant);
                TomatoMgr.TomatoMgrPuase = true;
                var result = MessageBox.Show(this, Properties.Resources.PlantFinishTipContent, Properties.Resources.PlantFinishTipCation,
                    MessageBoxButton.YesNo);
                TomatoMgr.TomatoMgrPuase = false;
                if (result == MessageBoxResult.Yes)
                {
                    selectPlant.StartGrow();
                    SwitchDoingView();
                }
                else
                {
                    SwitchPauseView();
                }
            }

            RefreshShowTomatoCount();
        }

        private void pausePlant_Click(object sender, RoutedEventArgs e)
        {
            if (curPlant == null)
            {
                throw new Exception("pausePlant_Click fail, curPlant is nil.");
            }
            if (curPlant.State == TOMATO_PLANT_STATE.Growing)
            {
                string pauseReason;
                var result = InputMessageBox.Show(Properties.Resources.PausePlantTipCaption,
                    Properties.Resources.PausePlantTipContent, out pauseReason);
                if (result.HasValue && result.Value)
                {
                    curPlant.Pause(pauseReason);

                    SwitchPauseView();
                }
            }
        }

        private void replayPlant_Click(object sender, RoutedEventArgs e)
        {
            if (curPlant == null)
            {
                throw new Exception("pausePlant_Click fail, curPlant is nil.");
            }
            if (curPlant.State == TOMATO_PLANT_STATE.Pause)
            {
                curPlant.Replay();

                SwitchDoingView();
            }
            else if (curPlant.State == TOMATO_PLANT_STATE.Reaped)
            {
                QueryStartRest();
            }
            else if (curPlant.State == TOMATO_PLANT_STATE.None)
            {
                curPlant.StartGrow();

                SwitchDoingView();
            }
        }

        private void giveupPlant_Click(object sender, RoutedEventArgs e)
        {
            if (curPlant == null)
            {
                throw new Exception("pausePlant_Click fail, curPlant is nil.");
            }
            if (curPlant.State == TOMATO_PLANT_STATE.Growing)
            {
                TomatoMgr.TomatoMgrPuase = true;
                var result = MessageBox.Show(this, Properties.Resources.GiveupTipCaption,
                    Properties.Resources.GiveupTipContent, MessageBoxButton.YesNo);
                TomatoMgr.TomatoMgrPuase = false;
                if (result == MessageBoxResult.Yes)
                {
                    string reason;
                    var inputResult = InputMessageBox.Show(Properties.Resources.GiveupTipInputCaption,
                        Properties.Resources.GiveupTipInputContent, out reason);
                    curPlant.Giveup(reason);

                    RemoveTomatoPlant(curPlant);
                    this.Close();
                }
            }
            else
            {
                RemoveTomatoPlant(curPlant);
                this.Close();
            }
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
            if (showBgStory == null || hideBgStory == null)
            {
                contentBorder.Visibility = Visibility.Visible;
            }
            else
            {
                if (contentBorder.Visibility != Visibility.Visible)
                {
                    hideBgStory.Stop();
                    showBgStory.Begin();
                }
            }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            if (showBgStory == null || hideBgStory == null)
            {
                contentBorder.Visibility = Visibility.Hidden;
            }
            else
            {
                if (contentBorder.Visibility != Visibility.Hidden)
                {
                    showBgStory.Stop();
                    hideBgStory.Begin(); 
                }
            }
        }

        
    }
}
