using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSovellus
{
    public class Class
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Class(string nm)
        {
            Name = nm;
        }

        public Class(int id, string nm)
        {
            ID = id;
            Name = nm;
        }

        public override string ToString()
        {
            string stringi = $"{Name}";

            return stringi;
        }
    }
}
