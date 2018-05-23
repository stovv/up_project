using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        Dictionary<int, Group> scheduel = new Dictionary<int, Group>();
        Dictionary<string, ListBox> time_list = new Dictionary<string, ListBox>();
        Dictionary<string, string> curr_day_new = new Dictionary<string, string>();
        Dictionary<string, string> curr_time = new Dictionary<string, string>();
        Dictionary<string, editor_com> curr_editor = new Dictionary<string, editor_com>();        

        string[] day_names = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        string[] day_names_locale = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };


        private ActionTabViewModal avm;
        public MainWindow()
        {
            InitializeComponent();
            SetAlignment();
            avm = new ActionTabViewModal();
            actionTabs.ItemsSource = avm.Tabs;
            avm.Populate();
            create_context_group(groupList);

        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // This event will be thrown when on a close image clicked
            clear_curr(avm.Tabs[actionTabs.SelectedIndex].Header);
            avm.Tabs.RemoveAt(actionTabs.SelectedIndex);
        }      
        private void open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Json Files (*.lson) | *.json";
            if (openFile.ShowDialog() == true)
            {
                scheduel = parse_schedule(openFile.FileName);
                setup_items();

            }
        }
        private void save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Up Files (*.up)|*.up|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true){
                save(saveFileDialog.FileName);
            }
        }
        private void get_Click(object sender, RoutedEventArgs e)
        {
            string path_temp = get_json();
            if ( path_temp != null)
            {
                scheduel = parse_schedule(path_temp);
                setup_items();
                msg_("Get success!");
            }
            else
            {
                msg_("Get failed");
            }
        }
        private void share_Click(object sender, RoutedEventArgs e)
        {
            //push_to_fire(json_);
            push_to_fire("temp\\temp.up");
        }
        private void groupList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = this.groupList.SelectedIndex;
            if (index < scheduel.Count)
            {
                int find = -1;
                foreach (ActionTab page in avm.Tabs)
                {
                    if (page.Header == scheduel[index].getName())
                    {
                        find = avm.get_index(page);
                        break;
                    }
                }
                if (find >= 0)
                {
                    actionTabs.SelectedIndex = find;
                }
                else
                {
                    //avm.add_new(scheduel[index].getName());

                    //new_tab(scheduel[index].getName(), scheduel[index]);
                    setup_gui(scheduel[index].getName(), scheduel[index]);
                    actionTabs.SelectedIndex = actionTabs.Items.Count - 1;

                    //setup_tab(tabPage, scheduel[index]);
                    //working_tabs.TabPages.Add(tabPage);
                    //working_tabs.SelectTab(working_tabs.TabPages.IndexOf(tabPage));

                }
            }
            else
            {
                
            }
            
            
        }
        //----help func
        private Dictionary<int, Group> parse_schedule(string filename)
        {
            string json = File.ReadAllText(filename);
            Dictionary<int, Group> rasp = new Dictionary<int, Group>();
            int counter = 0;
            JObject groups_j = JObject.Parse(json)["Группы"] as JObject;
            foreach (JProperty property in groups_j.Properties())
            {
                Group new_group = new Group();
                new_group.setName(property.Name);
                if (groups_j[property.Name]["Верхняя неделя"] != null)
                {
                    new_group.up_add(get_rasp((groups_j[property.Name]["Верхняя неделя"] as JObject), "up"));
                    //log_rasp(groups_j[property.Name]["Верхняя неделя"] as JObject);

                }
                if (groups_j[property.Name]["Нижняя неделя"] != null)
                {
                    new_group.down_add(get_rasp((groups_j[property.Name]["Нижняя неделя"] as JObject), "down"));//warning down/up_add addRequrse
                    //log_rasp(groups_j[property.Name]["Нижняя неделя"] as JObject);
                }
                rasp.Add(counter, new_group);
                counter++;
                // get a group keys and inside objects
            }

            return rasp;
        }
        private ArrayList get_rasp(JObject child, string type)
        {
            ArrayList days = new ArrayList();
            foreach (JProperty property in child.Properties())
            {
                Day new_day = new Day();
                new_day.setName(property.Name);
                JObject sub = child[property.Name] as JObject;
                foreach (JProperty sub_prop in sub.Properties())
                {
                    Subject subject = new Subject();
                    if (sub[sub_prop.Name]["name"] != null) subject.setName(sub[sub_prop.Name]["name"].Value<string>());
                    if (sub[sub_prop.Name]["aud"] != null) subject.setAud(sub[sub_prop.Name]["aud"].Value<string>());
                    if (sub[sub_prop.Name]["lector"] != null) subject.setLector(sub[sub_prop.Name]["lector"].Value<string>());
                    subject.setType(type);

                    new_day.add_subject(sub_prop.Name, subject);
                }
                days.Add(new_day);
            }
            return days;

        }
        public static void SetAlignment()
        {
            var ifLeft = SystemParameters.MenuDropAlignment;

            if (ifLeft)
            {
                // change to false
                var t = typeof(SystemParameters);
                var field = t.GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
                field.SetValue(null, false);

                ifLeft = SystemParameters.MenuDropAlignment;
            }
        }
        public static string get_json()
        {
            string ur = "https://alpha-e7b7e.firebaseio.com/.json";
            var request = WebRequest.CreateHttp(ur);
            request.ContentType = "application/json; charset=utf-8";

            var response = (HttpWebResponse)request.GetResponse();
            TextWriter writer = null;
            string temp_path = "temp\\temp.up";
            if (Directory.Exists(System.IO.Path.GetDirectoryName(temp_path)))
            {
                Directory.Delete(System.IO.Path.GetDirectoryName(temp_path), true);
            }
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(temp_path));
            try
            {
                writer = new StreamWriter(temp_path);
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    writer.Write(sr.ReadToEnd());
                }
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

            if (new FileInfo(temp_path).Length == 0)
            {
                return null;
            }
            return temp_path;
        }
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            if (Directory.Exists(System.IO.Path.GetDirectoryName(filePath)))
            {
                Directory.Delete(System.IO.Path.GetDirectoryName(filePath), true);
            }
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        public void write_file(string path, string js)
        {
            TextWriter writer = null;
            if (Directory.Exists(System.IO.Path.GetDirectoryName(path)))
            {
                Directory.Delete(System.IO.Path.GetDirectoryName(path), true);
            }
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            try
            {
                writer = new StreamWriter(path);
                writer.Write(js);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        private void push_to_fire(string path)
        {
            string ur = "https://alpha-e7b7e.firebaseio.com/.json";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ur);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(path))
                {
                    // Read the stream to a string, and write the string to the console.
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(sr.ReadToEnd());
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                msg_("The file could not be read:");
                Console.WriteLine(e.Message);
            }


        }
        public int get_group_index(string header)
        {
            return groupList.Items.IndexOf(header);
        }
        private void setup_items()
        {
            
            foreach (int key in scheduel.Keys)
            {
                groupList.Items.Add(scheduel[key].getName());
            }
            
        }

        private void add_curr_day_new(string day, ActionTab item)
        {
            string d;
            if (!curr_day_new.TryGetValue(item.Header, out d))
            {
                curr_day_new.Add(item.Header, day);
            }
            else
            {
                curr_day_new[item.Header] = day;
            }
        }
        private void clear_curr(string key)
        {
            curr_day_new.Remove(key);
            curr_editor.Remove(key);
            time_list.Remove(key);
            curr_time.Remove(key);
        }
        private void save(string path)
        {
            if (scheduel.Count > 0)
            {
                using (StreamWriter outputFile = new StreamWriter(path))
                {
                    outputFile.WriteLine("{\"Группы\": {");
                    foreach (Group group in scheduel.Values)
                    {
                        outputFile.WriteLine("\"" + group.getName() + "\" : {");//name group

                        outputFile.WriteLine("\"Верхняя неделя\" : {");//up
                        foreach (Day day in group.getUp_rasp())
                        {
                            outputFile.WriteLine("\"" + day.getName() + "\":{");
                            foreach (string time in day.getRasp().Keys)
                            {
                                outputFile.WriteLine("\"" + time + "\":{");
                                outputFile.WriteLine("\"name\":" + "\"" + day.getRasp()[time].getName() + "\",");
                                outputFile.WriteLine("\"aud\":" + "\"" + day.getRasp()[time].getAud() + "\",");
                                outputFile.WriteLine("\"lector\":" + "\"" + day.getRasp()[time].getLector() + "\"");
                                if (time == day.getRasp().Keys.Last())
                                {
                                    outputFile.WriteLine("}");
                                }
                                else
                                {
                                    outputFile.WriteLine("},");
                                }
                            }

                            if (group.getUp_rasp().IndexOf(day) == group.getUp_rasp().Count - 1)
                            {
                                outputFile.WriteLine("}");
                            }
                            else
                            {
                                outputFile.WriteLine("},");
                            }
                        }
                        outputFile.WriteLine("},");//up

                        outputFile.WriteLine("\"Нижняя неделя\" : {");//down

                        foreach (Day day in group.getDown_rasp())
                        {
                            outputFile.WriteLine("\"" + day.getName() + "\":{");
                            foreach (string time in day.getRasp().Keys)
                            {
                                outputFile.WriteLine("\"" + time + "\":{");
                                outputFile.WriteLine("\"name\":" + "\"" + day.getRasp()[time].getName() + "\",");
                                outputFile.WriteLine("\"aud\":" + "\"" + day.getRasp()[time].getAud() + "\",");
                                outputFile.WriteLine("\"lector\":" + "\"" + day.getRasp()[time].getLector() + "\"");
                                if (time == day.getRasp().Keys.Last())
                                {
                                    outputFile.WriteLine("}");
                                }
                                else
                                {
                                    outputFile.WriteLine("},");
                                }
                            }

                            if (group.getDown_rasp().IndexOf(day) == group.getDown_rasp().Count - 1)
                            {
                                outputFile.WriteLine("}");
                            }
                            else
                            {
                                outputFile.WriteLine("},");
                            }
                        }

                        outputFile.WriteLine("}");//down



                        if (group.getName() == scheduel.Values.Last().getName())
                        {
                            outputFile.WriteLine("}");
                        }
                        else
                        {
                            outputFile.WriteLine("},");
                        }//name group
                    }
                    outputFile.WriteLine("}");
                    outputFile.WriteLine("}");
                }
            }
            else
            {
                msg_("No open scheduel..");
            }
            
        }
        private void msg_(string text)
        {
            MessageBox.Show(text);
        }

        //------new code
        public void click_time_new (object sender, MouseEventArgs e)
        {
            ListBox list = (ListBox)sender;
            int index_select = list.SelectedIndex;
            string st = list.SelectedItem as string;
            ActionTab item = avm.Tabs[actionTabs.SelectedIndex];
            string[] time_unformat = unformatter(st);
            if (time_unformat[1] == "up" || time_unformat[1] == "all")
            {
                int index_day = find_day(scheduel[get_group_index(item.Header)].getUp_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[item.Header])]);
                if (index_day >= 0)
                {
                    Day day = scheduel[get_group_index(item.Header)].getUp_rasp()[index_day] as Day;
                    setup_editor(item, day.getRasp()[time_unformat[0]], time_unformat);
                    string sub_name = day.getRasp()[time_unformat[0]].getName();
                    string time_format;
                    if (!curr_time.TryGetValue(item.Header, out time_format))
                    {
                        curr_time.Add(item.Header, st);
                    }
                    else
                    {
                        curr_time[item.Header] = st;
                    }
                }
              
            }else if (time_unformat[1] == "down")
            {
                int index_day = find_day(scheduel[get_group_index(item.Header)].getDown_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[item.Header])]);
                if (index_day >= 0)
                {
                    Day day = scheduel[get_group_index(item.Header)].getDown_rasp()[index_day] as Day;
                    setup_editor(item, day.getRasp()[time_unformat[0]], time_unformat);
                    string sub_name = day.getRasp()[time_unformat[0]].getName();
                    string time_format;
                    if (!curr_time.TryGetValue(item.Header, out time_format))
                    {
                        curr_time.Add(item.Header, st);
                    }
                    else
                    {
                        curr_time[item.Header] = st;
                    }
                }
            }
            else
            {
                setup_editor(item);
            }
        }
        public void click_day(object sender, MouseEventArgs e)
        {
            ListBox list = (ListBox)sender;
            int index = list.SelectedIndex;
            ActionTab item = avm.Tabs[actionTabs.SelectedIndex];
            int index_group = get_group_index(item.Header);

            add_curr_day_new(list.SelectedItem.ToString(), item);
            int up_day_index = find_day(scheduel[index_group].getUp_rasp(), day_names_locale[index]);
            int down_day_index = find_day(scheduel[index_group].getDown_rasp(), day_names_locale[index]);
            if (up_day_index >= 0 && down_day_index >= 0)
            {
                create_time_list(scheduel[index_group].getUp_rasp()[up_day_index] as Day, scheduel[index_group].getDown_rasp()[down_day_index] as Day, item);
            }
            else if (up_day_index >= 0)
            {
                create_time_list(scheduel[index_group].getUp_rasp()[up_day_index] as Day, item);
            }
            else if (down_day_index >= 0)
            {
                create_time_list(scheduel[index_group].getDown_rasp()[down_day_index] as Day, item);
            }
            else
            {
                create_time_list(new Day(), item);
            }

            //create_time_list(scheduel[index_group].getUp_rasp()[index] as Day, scheduel[index_group].getDown_rasp()[index] as Day, item);
            //create_edit_field(scheduel[index_group].getUp_rasp()[index] as Day, , item);
        }
        public bool rav(Subject one, Subject two)
        {
            if (one.getName() == two.getName()  &&
                one.getAud() == two.getAud()    &&
                one.getLector() == two.getLector())
            {
                return true;
            }
            return false;
        }
        public void create_time_list(Day day, ActionTab tab)
        {
            ListBox list_b = null;
            if (!time_list.TryGetValue(tab.Header, out list_b))
            {
                ListBox timeList = new ListBox();
                timeList.Margin = new Thickness(10, 20, 0, 0);
                timeList.Width = 150;
                timeList.MouseDoubleClick += click_time_new;
                create_context_subject(timeList);
                time_list.Add(tab.Header, timeList);
                (tab.Content as UpControl).add_new(time_list[tab.Header]);
            }
            time_list[tab.Header].Items.Clear();
            
            foreach(string time in day.getRasp().Keys)
            {
                time_list[tab.Header].Items.Add(formatter(time, day.getRasp()[time].getType()));
            }
        }
        public void create_time_list(Day up, Day down, ActionTab tab)
        {
            ListBox list_b = null;
            if (!time_list.TryGetValue(tab.Header, out list_b))
            {
                ListBox timeList = new ListBox();
                timeList.Margin = new Thickness(10, 20, 0, 0);
                timeList.Width = 150;
                timeList.MouseDoubleClick += click_time_new;
                create_context_subject(timeList);
                time_list.Add(tab.Header, timeList);
                (tab.Content as UpControl).add_new(time_list[tab.Header]);
            }

            time_list[tab.Header].Items.Clear();
            Dictionary<string, Subject> down_buff = new Dictionary<string, Subject>();
            foreach (var sub in down.getRasp())
                down_buff.Add(sub.Key, sub.Value);
            
            foreach (string up_time in up.getRasp().Keys)
            {
                bool find = false;
                foreach (string down_time in down_buff.Keys)
                {
                    if (up_time == down_time)
                    {
                        if (rav(up.getRasp()[up_time], down.getRasp()[down_time]))
                        {
                            time_list[tab.Header].Items.Add(formatter(up_time, "all"));
                            find = true;
                            down_buff.Remove(down_time);
                            break;
                        }
                        else
                        {
                            time_list[tab.Header].Items.Add(formatter(up_time, up.getRasp()[up_time].getType()));
                            time_list[tab.Header].Items.Add(formatter(down_time, down_buff[down_time].getType()));
                            down_buff.Remove(down_time);
                            find = true;
                            break;
                        }
                    }
                }
                if (!find)
                {
                    time_list[tab.Header].Items.Add(formatter(up_time, up.getRasp()[up_time].getType()));
                }
            }
            foreach(string time in down_buff.Keys)
            {
                time_list[tab.Header].Items.Add(formatter(time, down_buff[time].getType()));
            }

        }
        public int find_day(ArrayList list, string name)
        {
            int index = 0;
            foreach(Day day_ in list)
            {
                if (name == day_.getName())
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
        private void setup_gui(string header, Group group)
        {
            UpControl content = new UpControl();
            ListBox days = new ListBox();
            days.Margin = new Thickness(10, 20, 0, 0);
            days.Width = 100;
            days.MouseDoubleClick += click_day;
            foreach (string name_day in day_names)
            {
                days.Items.Add(name_day);
            }
            content.add_new(days);
            avm.add_new(content, header);
        }
        private void setup_editor(ActionTab tab, Subject sub, string [] time_unformat)
        {
            editor_com editor = null;
            if (!curr_editor.TryGetValue(tab.Header, out editor))
            {
                editor_com ed = new editor_com();
                ed.Margin = new Thickness(10, 20, 0, 0);
                curr_editor.Add(tab.Header, ed);
                ed.apply_btn.MouseDoubleClick += click_apply_new;
                (tab.Content as UpControl).add_new(curr_editor[tab.Header]);
            }
            curr_editor[tab.Header].set_field(sub.getName(), time_unformat[0], sub.getAud(), sub.getLector());
            curr_editor[tab.Header].check(time_unformat[1]);
        }
        private void setup_editor(ActionTab tab)
        {
            editor_com editor = null;
            if (!curr_editor.TryGetValue(tab.Header, out editor))
            {
                editor_com ed = new editor_com();
                ed.Margin = new Thickness(10, 20, 0, 0);
                curr_editor.Add(tab.Header, ed);
                ed.apply_btn.MouseDoubleClick += click_apply_new;
                (tab.Content as UpControl).add_new(curr_editor[tab.Header]);
            }
            curr_editor[tab.Header].clear();
        }
        public string formatter(string time, string type)
        {
            string t = time + "-(" + type + ")";
            return t;

        }
        public string[] unformatter(string time_format)
        {
            string []un = new string[2];
            int indx = time_format.IndexOf('(');
            un[0] = time_format.Substring(0, indx-1);
            un[1] = time_format.Substring(indx+1, time_format.Length-2 - indx);
            return un;

        }
        //----help func
        public void click_apply_new(object sender, MouseEventArgs e)
        {
            ActionTab item = avm.Tabs[actionTabs.SelectedIndex];
            string[] time_un = unformatter(curr_time[item.Header]);
            if (time_un[1] == "up")
            {
                int index_day = find_day(scheduel[get_group_index(item.Header)].getUp_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[item.Header])]);
                (scheduel[get_group_index(item.Header)].getUp_rasp()[index_day] as Day).getRasp()[time_un[0]].setName(curr_editor[item.Header].name_box.Text);
                (scheduel[get_group_index(item.Header)].getUp_rasp()[index_day] as Day).getRasp()[time_un[0]].setAud(curr_editor[item.Header].aud_box.Text);
                (scheduel[get_group_index(item.Header)].getUp_rasp()[index_day] as Day).getRasp()[time_un[0]].setLector(curr_editor[item.Header].lector_box.Text);

                Subject buffer = (scheduel[get_group_index(item.Header)].getUp_rasp()[index_day] as Day).getRasp()[time_un[0]];
                (scheduel[get_group_index(item.Header)].getUp_rasp()[index_day] as Day).getRasp().Remove(time_un[0]);
                (scheduel[get_group_index(item.Header)].getUp_rasp()[index_day] as Day).getRasp().Add(curr_editor[item.Header].time_box.Text, buffer);
                time_list[item.Header].Items.Remove(curr_time[item.Header]);
                curr_time[item.Header] = formatter(curr_editor[item.Header].time_box.Text, curr_editor[item.Header].get_selected_type());
                time_list[item.Header].Items.Add(curr_time[item.Header]);
                setup_editor(item, (scheduel[get_group_index(item.Header)].getUp_rasp()[index_day] as Day).getRasp()[unformatter(curr_time[item.Header])[0]], unformatter(curr_time[item.Header]));
            }
            else if (time_un[1] == "down")
            {
                int index_day = find_day(scheduel[get_group_index(item.Header)].getDown_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[item.Header])]);
                (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day] as Day).getRasp()[time_un[0]].setName(curr_editor[item.Header].name_box.Text);
                (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day] as Day).getRasp()[time_un[0]].setAud(curr_editor[item.Header].aud_box.Text);
                (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day] as Day).getRasp()[time_un[0]].setLector(curr_editor[item.Header].lector_box.Text);

                Subject buffer = (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day] as Day).getRasp()[time_un[0]];
                (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day] as Day).getRasp().Remove(time_un[0]);
                (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day] as Day).getRasp().Add(curr_editor[item.Header].time_box.Text, buffer);
                time_list[item.Header].Items.Remove(curr_time[item.Header]);
                curr_time[item.Header] = formatter(curr_editor[item.Header].time_box.Text, curr_editor[item.Header].get_selected_type());
                time_list[item.Header].Items.Add(curr_time[item.Header]);
                setup_editor(item, (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day] as Day).getRasp()[unformatter(curr_time[item.Header])[0]], unformatter(curr_time[item.Header]));

            }
            else if (time_un[1] == "all")
            {
                int index_day_up = find_day(scheduel[get_group_index(item.Header)].getUp_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[item.Header])]);
                int index_day_down = find_day(scheduel[get_group_index(item.Header)].getDown_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[item.Header])]);

                //up
                (scheduel[get_group_index(item.Header)].getUp_rasp()[index_day_up] as Day).getRasp()[time_un[0]].setName(curr_editor[item.Header].name_box.Text);
                (scheduel[get_group_index(item.Header)].getUp_rasp()[index_day_up] as Day).getRasp()[time_un[0]].setAud(curr_editor[item.Header].aud_box.Text);
                (scheduel[get_group_index(item.Header)].getUp_rasp()[index_day_up] as Day).getRasp()[time_un[0]].setLector(curr_editor[item.Header].lector_box.Text);

                //down
                (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day_down] as Day).getRasp()[time_un[0]].setName(curr_editor[item.Header].name_box.Text);
                (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day_down] as Day).getRasp()[time_un[0]].setAud(curr_editor[item.Header].aud_box.Text);
                (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day_down] as Day).getRasp()[time_un[0]].setLector(curr_editor[item.Header].lector_box.Text);

                Subject buffer = (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day_down] as Day).getRasp()[time_un[0]];
                (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day_down] as Day).getRasp().Remove(time_un[0]);
                (scheduel[get_group_index(item.Header)].getDown_rasp()[index_day_down] as Day).getRasp().Add(curr_editor[item.Header].time_box.Text, buffer);
                time_list[item.Header].Items.Remove(curr_time[item.Header]);
                curr_time[item.Header] = formatter(curr_editor[item.Header].time_box.Text, "all");
                time_list[item.Header].Items.Add(curr_time[item.Header]);
                setup_editor(item, (scheduel[get_group_index(item.Header)].getUp_rasp()[index_day_up] as Day).getRasp()[unformatter(curr_time[item.Header])[0]], unformatter(curr_time[item.Header]));
            }

            save("temp\\temp.up");
            MessageBox.Show("Apply");
        }

        public void create_context_group(ListBox item)
        {
            // Create Context Menu
            ContextMenu contextMenu1;
            contextMenu1 = new ContextMenu();

            //Create menu items
            MenuItem menuItem1;
            menuItem1 = new MenuItem();
            contextMenu1.Items.Add(menuItem1);
            menuItem1.Header = "Create";
            menuItem1.Click += delegate {
                string name_group = "";
                TextEnter group_name = new TextEnter("Please enter your group name:", "");
                if (group_name.ShowDialog() == true)
                    name_group = group_name.Answer;
                if (name_group != "")
                {
                    Group gr = new Group();
                    gr.setName(name_group);
                    scheduel.Add(item.Items.Count,gr);
                    item.Items.Add(name_group);
                }
                save("temp\\temp.up");
            };

            MenuItem menuItem2;
            menuItem2 = new MenuItem();
            contextMenu1.Items.Add(menuItem2);
            menuItem2.Header = "Delete";
            menuItem2.Click += delegate {
                {
                    if (item.SelectedIndex >= 0)
                    {
                        scheduel.Remove(get_group_index(item.SelectedItem as string));
                        item.Items.Remove(item.SelectedItem);
                        save("temp\\temp.up");
                    }
                };
            };

            item.ContextMenu = contextMenu1;

        }

        public void create_context_subject(ListBox item)
        {
            // Create Context Menu
            ContextMenu contextMenu1;
            contextMenu1 = new ContextMenu();

            //Create menu items
            MenuItem menuItem1;
            menuItem1 = new MenuItem();
            contextMenu1.Items.Add(menuItem1);
            menuItem1.Header = "Create";
            menuItem1.Click += delegate {
                string[] subj = null;
                SubEnter sub = new SubEnter();
                if (sub.ShowDialog() == true)
                    subj = sub.get_answer();

                if (subj != null)
                {
                    Subject subject = new Subject();
                    subject.setName(subj[2]);
                    subject.setAud(subj[3]);
                    subject.setLector(subj[4]);
                    subject.setType(subj[5]);
                    if (subj[5] == "up")
                    {
                        int index_day = find_day(scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)]
                                .getUp_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[(actionTabs.SelectedItem as ActionTab).Header])]);
                        string time = formatter(subj[0] + "-" + subj[1], subj[5]);
                        (scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)].getUp_rasp()[index_day] as Day).getRasp().Add(subj[0] + "-" + subj[1], subject);
                        item.Items.Add(time);

                    }
                    else if (subj[5] == "down")
                    {
                        int index_day = find_day(scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)]
                                .getDown_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[(actionTabs.SelectedItem as ActionTab).Header])]);
                        string time = formatter(subj[0] + "-" + subj[1], subj[5]);
                        (scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)].getDown_rasp()[index_day] as Day).getRasp().Add(subj[0] + "-" + subj[1], subject);
                        item.Items.Add(time);

                    }
                    else if (subj[5] == "all")
                    {
                        int index_day_up = find_day(scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)]
                                .getDown_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[(actionTabs.SelectedItem as ActionTab).Header])]);
                        string time = formatter(subj[0] + "-" + subj[1], subj[5]);
                        (scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)].getDown_rasp()[index_day_up] as Day).getRasp().Add(subj[0] + "-" + subj[1], subject);
                        int index_day_down = find_day(scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)]
                                .getUp_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[(actionTabs.SelectedItem as ActionTab).Header])]);
                        (scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)].getUp_rasp()[index_day_down] as Day).getRasp().Add(subj[0] + "-" + subj[1], subject);
                        item.Items.Add(time);
                    }
                }
                save("temp\\temp.up");
            };

            MenuItem menuItem2;
            menuItem2 = new MenuItem();
            contextMenu1.Items.Add(menuItem2);
            menuItem2.Header = "Delete";
            menuItem2.Click += delegate {
                {
                    if (item.SelectedIndex >= 0)
                    {
                        string[] time_un = unformatter(item.SelectedItem as string);
                        if (time_un[1] == "up")
                        {
                            int index_day = find_day(scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)]
                                .getUp_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[(actionTabs.SelectedItem as ActionTab).Header])]);
                            (scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)].getUp_rasp()[index_day] as Day).getRasp().Remove(time_un[0]);
                        }
                        else if (time_un[1] == "down")
                        {
                            int index_day = find_day(scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)]
                                .getDown_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[(actionTabs.SelectedItem as ActionTab).Header])]);
                            (scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)].getDown_rasp()[index_day] as Day).getRasp().Remove(time_un[0]);
                        }
                        else if (time_un[1] == "all")
                        {
                            int index_day_up = find_day(scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)]
                                .getUp_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[(actionTabs.SelectedItem as ActionTab).Header])]);
                            (scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)].getUp_rasp()[index_day_up] as Day).getRasp().Remove(time_un[0]);
                            int index_day_down = find_day(scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)]
                                .getDown_rasp(), day_names_locale[Array.IndexOf(day_names, curr_day_new[(actionTabs.SelectedItem as ActionTab).Header])]);
                            (scheduel[get_group_index((actionTabs.SelectedItem as ActionTab).Header)].getDown_rasp()[index_day_down] as Day).getRasp().Remove(time_un[0]);
                        }
                        item.Items.Remove(item.SelectedItem);
                        save("temp\\temp.up");
                    }
                };
            };

            item.ContextMenu = contextMenu1;
        }
        public void create_item(ListBox list)
        {
            if (list.SelectedIndex >= 0)
            {
                list.Items.Remove(list.SelectedItem);
            }
        }
    }


}
