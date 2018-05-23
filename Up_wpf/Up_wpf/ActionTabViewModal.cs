using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Up_wpf
{
    public class ActionTabViewModal
    {
        // These Are the tabs that will be bound to the TabControl 
        public ObservableCollection<ActionTab> Tabs { get; set; }
        public ActionTabViewModal()
        {
            Tabs = new ObservableCollection<ActionTab>();
        }

        public void Populate()
        {
            UpControl web_c = new UpControl();
            WebBrowser browser = new WebBrowser();
            //browser.Navigate(@"www.google.com");
            browser.HorizontalAlignment = HorizontalAlignment.Stretch;
            browser.VerticalAlignment = VerticalAlignment.Stretch;
            web_c.add_new(browser);
            // Add A tab to TabControl With a specific header and Content(UserControl)
            Tabs.Add(new ActionTab { Header = "Welcome!", Content = new UpControl() });
            // Add A tab to TabControl With a specific header and Content(UserControl)
            //Tabs.Add(new ActionTab { Header = "UserControl 2", Content = new TestUserControl() });
        }
        public void add_new(string header)
        {
            Tabs.Add(new ActionTab { Header = header, Content = new UpControl() });
        }
        
        public void add_new(UpControl content, string header) {
            Tabs.Add(new ActionTab { Header = header, Content = content });
        }
        
        public int get_index(ActionTab tab)
        {
            return Tabs.IndexOf(tab);
        }

    }
}
