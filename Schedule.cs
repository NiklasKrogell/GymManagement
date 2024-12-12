using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GymSovellus
{
    public class Schedule
    {
        Gym gymi = new Gym();
        public int ScheduleID { get; set; }
        public DateTime Time { get; set; }

        public DateTime EndTime { get; set; }
        public int Class_ref { get; set; }
        public int Trainer_ref { get; set; }

        public Schedule(int clas, int trnr, DateTime time)
        {
            Class_ref = clas;
            Trainer_ref = trnr;
            Time = time;
            EndTime = time.AddHours(1);
        }

        public Schedule(int SID, int clas, int trnr, DateTime time):this(clas, trnr, time)
        {
            ScheduleID = SID;
            
        }

        public override string ToString()
        {
            Class c = gymi.GetClass("ClassID", Class_ref.ToString());
            Trainer t = gymi.GetTrnr("TrainerID", Trainer_ref.ToString());
            string sch = $"{c.Name}, {t.FirstName} {t.LastName}, {Time.ToString("dd.MM.yyyy HH:mm")}";

            return sch;
        }

    }
}
