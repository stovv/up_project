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
    /// Interaction logic for UpControl.xaml
    /// </summary>
    public partial class UpControl : UserControl
    {
        public UpControl()
        {
            InitializeComponent();
            
        }
        public void add_new(UIElement obj)
        {
            grid_.Children.Add(obj);
        }
    }
}
