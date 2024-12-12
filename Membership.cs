using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSovellus
{
    public class Membership
    {
        public int ID { get; }
        public string Name {  get; }
        public DateTime Start {  get; }
        public DateTime End { get; }
        public int Customer_ref {  get; }
        public Membership(int id, string nm, DateTime st, DateTime end, int Cust)
        {
            ID = id;
            Name = nm;
            Start = st;
            End = end;
            Customer_ref = Cust;
        }

        public Membership(int id, int nr) 
        {
            Customer_ref = id;

            switch(nr)
            {
                case 1:
                    Name = "Monthly";
                    Start = DateTime.Now;
                    End = Start.AddMonths(1);
                    break;
                case 2:
                    Name = "Half a year";
                    Start = DateTime.Now;
                    End = Start.AddMonths(6);
                    break;
                case 3:
                    Name = "Yearly";
                    Start = DateTime.Now;
                    End = Start.AddYears(1);
                    break;
                default:
                    break;
            }
        }

        public override string ToString()
        {
            if ((End - DateTime.Now).Days > 0) {
                string s = $"{Name}, days left: {(End - DateTime.Now).Days}";
                return s;
            }
            else
            {
                string s = "Membership expired";
                return s;
            }
        }
    }
}
