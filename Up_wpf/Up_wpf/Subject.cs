using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Up_wpf
{
    public class Subject
    {
        private string name;
        private string aud;
        private string lector;
        private string type;
        public Subject() { }
        public Subject(string name, string aud, string lector, string type)
        {
            this.name = name;
            this.aud = aud;
            this.lector = lector;
            this.type = type;
        }
        public string getAud()
        {
            return aud;
        }

        public string getName()
        {
            return name;
        }

        public string getLector()
        {
            return lector;
        }

        public void setAud(string aud)
        {
            this.aud = aud;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public void setLector(string lector)
        {
            this.lector = lector;
        }

        public string getType()
        {
            return type;
        }
        public void setType(string type)
        {
            this.type = type;
        }
    }
}
