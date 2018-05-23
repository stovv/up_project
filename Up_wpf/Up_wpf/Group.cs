using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Up_wpf
{
    public class Group
    {
        private string name;
        private ArrayList up_rasp;
        private ArrayList down_rasp;

        public Group()
        {
            up_rasp = new ArrayList();
            down_rasp = new ArrayList();
        }
        public void setName(string name)
        {
            this.name = name;
        }

        public string getName()
        {
            return name;
        }
        public void up_add(ArrayList days)
        {
            up_rasp.AddRange(days);

        }
        public void down_add(ArrayList days)
        {
            down_rasp.AddRange(days);

        }
        public ArrayList getDown_rasp()
        {
            return down_rasp;
        }

        public ArrayList getUp_rasp()
        {
            return up_rasp;
        }
    }
}
