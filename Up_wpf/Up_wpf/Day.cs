using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Up_wpf
{
    public class Day
    {
        private string name;
        private Dictionary<string, Subject> rasp;
        public Day()
        {
            rasp = new Dictionary<string, Subject>();
        }

        public string getName()
        {
            return name;
        }
        public void setName(string name)
        {
            this.name = name;
        }

        public void add_subject(string time, Subject subject)
        {//time format (7_30-9_10)
            rasp.Add(time, subject);
        }

        public Dictionary<string, Subject>  getRasp()
        {
            return rasp;
        }
    }
}
