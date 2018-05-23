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

namespace Up_wpf
{
    /// <summary>
    /// Interaction logic for editor_com.xaml
    /// </summary>
    public partial class editor_com : UserControl
    {
        public editor_com()
        {
            InitializeComponent();
        }
        public void set_field(string name, string time, string aud, string lector)
        {
            name_box.Text = name;
            time_box.Text = time;
            aud_box.Text = aud;
            lector_box.Text = lector;
        }
        public void check(string name)
        {
            if (name == "up")
            {
                up_.IsChecked = true;
                down_.IsChecked = false;
                all_.IsChecked = false;
            }
            else if (name == "down")
            {
                up_.IsChecked = false;
                down_.IsChecked = true;
                all_.IsChecked = false;
            }
            else if (name == "all")
            {
                up_.IsChecked = false;
                down_.IsChecked = false;
                all_.IsChecked = true;
            }
            else
            {
                //ERROR
            }

        }
        public void clear()
        {
            name_box.Text = "";
            time_box.Text = "";
            aud_box.Text = "";
            lector_box.Text = "";
            up_.IsChecked = false;
            down_.IsChecked = false;
            all_.IsChecked = true;
        }

        public string get_selected_type()
        {
            if (up_.IsChecked == true)
                return "up";
            if (all_.IsChecked == true)
                return "all";
            if (down_.IsChecked == true)
                return "down";
            return null;
        }
        
    }
}
