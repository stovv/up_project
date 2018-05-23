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

namespace Up_wpf
{
    /// <summary>
    /// Interaction logic for SubEnter.xaml
    /// </summary>
    public partial class SubEnter : Window
    {
        public SubEnter()
        {
            InitializeComponent();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            time_one.SelectAll();
            time_one.Focus();
        }

        public string [] get_answer()
        {
            string[] answers = new string[6];
            answers[0] = time_one.Text;
            answers[1] = time_two.Text;
            answers[2] = name.Text;
            answers[3] = aud.Text;
            answers[4] = lector.Text;
            if (up_.IsChecked == true)
            {
                answers[5] = "up";
            }
            else if(down_.IsChecked == true)
            {
                answers[5] = "down";
            }else
            {
                answers[5] = "all";
            }
                
            return answers;
        }
    }
}
