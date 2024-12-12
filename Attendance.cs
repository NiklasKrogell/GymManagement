using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSovellus
{
    public class Attendance
    {
        public int Membership_ref {  get; set; }
        public int Schedule_ref { get; set; }
        public Attendance(int cust, int sch) 
        {
            Membership_ref = cust;
            Schedule_ref = sch;
        }
    }
}
