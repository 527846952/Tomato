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
    /// InputMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class InputMessageBox : Window
    {
        public string InputContent
        {
            get
            {
                if (inputContent == null)
                {
                    return string.Empty;
                }
                return inputContent.Text;
            }
        }

        private InputMessageBox()
        {
            InitializeComponent();
        }

        public static bool? Show(string caption, string tip, out string input)
        {
            TomatoMgr.TomatoMgrPuase = true;

            var inputBox = new InputMessageBox();
            inputBox.Title = caption;
            inputBox.inputTip.Content = tip;
            var result = inputBox.ShowDialog();
            input = inputBox.InputContent;

            TomatoMgr.TomatoMgrPuase = false;

            return result;
        }

        private void finishInput_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
