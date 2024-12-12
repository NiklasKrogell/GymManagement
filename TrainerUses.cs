using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSovellus
{
    internal class TrainerUses
    {
        public int Membership_ref {  get; set; }
        public DateTime VisitTime { get; set; }
        public int Trainer_ref { get; set; }
        public TrainerUses(int cust, DateTime time, int trnr) 
        {
            Membership_ref = cust;
            VisitTime = time;
            Trainer_ref = trnr;
        }
    }
}
