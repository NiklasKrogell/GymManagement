using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSovellus
{
    public class GymVisit
    {
        public int VisitID { get; }
        public int Membership_ref { get; set; }
        public DateTime VisitTime { get; set; }
        public GymVisit(int id, int cust, DateTime time) 
        {
            VisitID = id;
            Membership_ref = cust;
            VisitTime = time;
        }
    }
}
