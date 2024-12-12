using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Reflection.Emit;

namespace GymSovellus
{
    internal class Gym
    {
        DataService MyDS = DataService.GetDataService();

        public void AddTrainer(string fn, string ln, string email)
        {
            string[] fields = new string[] { "FirstName", "LastName", "Email" };
            string[] values = new string[] { fn, ln, email };
            MyDS.InsertInto("Trainer", fields, values);
        }//Valmis

        public void AddCust(string fn, string ln, string email)
        {
            string[] fields = new string[] { "FirstName", "LastName", "Email" };
            string[] values = new string[] { fn, ln, email };
            MyDS.InsertInto("Customer", fields, values);
        }//Valmis

        public void AddMemb(Membership memb)
        {
            string[] fields = new string[] { "MembName", "StartDate", "EndDate", "Customer_ref" };
            string[] values = new string[] { memb.Name, memb.Start.ToString(), memb.End.ToString(), memb.Customer_ref.ToString() };
            MyDS.AddMembShip(memb, fields, values);
        }//Valmis

        public void AddClass(Class clas)
        {
            string[] fields = new string[] {  "ClassName" };
            string[] values = new string[] { clas.Name };
            MyDS.InsertInto("Class", fields, values);
        }//Valmis

        public void AddSchedule(Schedule sch)
        {
            string[] fields = new string[] { "Class_ref", "Trainer_ref", "VisitTime" };
            string[] values = new string[] { sch.Class_ref.ToString(), sch.Trainer_ref.ToString(), sch.Time.ToString() };
            MyDS.InsertInto("Schedule", fields, values);
        }//Valmis

        public bool AttendClass(Attendance att)
        {
            Customer cust = GetCust("Membership_ref", att.Membership_ref.ToString());
            Schedule sch = GetSchedule("ScheduleID", att.Schedule_ref.ToString());
            if (MembValid(cust))
            {
                if(AttendValid(cust))
                {
                    string[] fields = new string[] {"Membership_ref", "Schedule_ref"};
                    string[] values = new string[] { cust.Membership_ref.ToString(), sch.ScheduleID.ToString() };
                    MyDS.InsertInto("Attendance", fields, values);
                    CustVisit(cust, sch.Time);
                    return true;
                }
                else {  return false; }
            }
            else { return false; }
        }//Valmis

        public bool CustVisit(Customer cust, DateTime time)
        {
            string[] fields = new string[] { "Membership_ref", "VisitTime" };
            string[] values = new string[] {cust.Membership_ref.ToString(), time.ToString()};
            bool IsMember = MembValid(cust);
            if(IsMember)
            {
                MyDS.InsertInto("GymVisits", fields, values);
                return true;
            }
            else
            {
                return false;
            }
        }//Valmis

        public bool UseTrainer(Customer cust, DateTime time)
        {
            if (MembValid(cust))
            {
                if (cust.PersonalTrainer > 0)
                {
                    if (TrainerValid(cust))
                    {
                        string[] fields = new string[] { "Membership_ref", "VisitTime", "Trainer_ref" };
                        string[] values = new string[] { cust.Membership_ref.ToString(), time.ToString(), cust.PersonalTrainer.ToString() };
                        MyDS.InsertInto("TrainerUses", fields, values);
                        CustVisit(cust, time);
                        return true;
                    }
                    else { return false; }
                }else { return false; }                   
            }
            else { return false; }
        }//Valmis

        public Customer GetCust(string field, string value)
        {
            Dictionary<string, object> data = MyDS.SelectWhere("Customer", field, value);

            Customer cust = new Customer(Convert.ToInt32(data["CustomerID"]), data["FirstName"].ToString(), data["LastName"].ToString(), data["Email"].ToString(), data["Membership_ref"].ToString(), data["Trainer_ref"].ToString());

            return cust;
        }//Valmis

        public Trainer GetTrnr(string field, string value)
        {
            Dictionary<string, object> data = MyDS.SelectWhere("Trainer", field, value);

            Trainer trnr = new Trainer(Convert.ToInt32(data["TrainerID"]), data["FirstName"].ToString(), data["LastName"].ToString(), data["Email"].ToString());

            return trnr;
        }//Valmis

        public Membership GetMembShip(string field, string value)
        {
            Dictionary<string, object> data = MyDS.SelectWhere("Membership", field, value);

            Membership memb = new Membership(Convert.ToInt32(data["MembershipID"]), data["MembName"].ToString(), Convert.ToDateTime(data["StartDate"]), Convert.ToDateTime(data["EndDate"]), Convert.ToInt32(data["Customer_ref"]));

            return memb;
        }//Valmis

        public Class GetClass (string field, string value)
        {
            Dictionary<string, object> data = MyDS.SelectWhere("Class", field, value);

            Class clas = new Class(Convert.ToInt32(data["ClassID"]), data["ClassName"].ToString());

            return clas;
        }//Valmis

        public Schedule GetSchedule(string field, string value)
        {
            Dictionary<string, object> data = MyDS.SelectWhere("Schedule", field, value);

            Schedule sch = new Schedule(Convert.ToInt32(data["ScheduleID"]), Convert.ToInt32(data["Class_ref"]), Convert.ToInt32(data["Trainer_ref"]), Convert.ToDateTime(data["VisitTime"]));

            return sch;
        }//Valmis

        public List<Customer> GetCustList()
        {
            return MyDS.GetAllCustomers();
        }

        public List<Trainer> GetTrnrList()
        {
            return MyDS.GetAllTrainers();
        }

        public List<Class> GetClassList()
        {
            return MyDS.GetAllClasses();
        }

        public List<Schedule> GetScheduleList()
        {
            return MyDS.GetAllSchedules();
        }

        public string GetVisits(Customer cust)
        {
            string vis = string.Format("{0,-25}\n", "Time");
            vis += new string('-', 103) + "\n";

            List<GymVisit> visits = MyDS.GetVisits(cust);

            foreach (GymVisit visit in visits)
            {
                vis += visit.VisitTime.ToString("dd.MM.yyyy HH:mm") + "\n";
            }

            return vis;
        }//Valmis

        public string GetAttendances(Customer cust)
        {
            string att = string.Format("{0,-25} {1,-25} {2,-28}\n", "Class", "Time", "Trainer" );
            att += new string('-', 103) + "\n";

            List<Attendance> atts = MyDS.GetAttendances(cust);

            foreach (Attendance attendance in atts)
            {
                att += PrintAttendance(attendance);
            }

            return att;
        }//Valmis

        public string GetTrainerUses(Customer cust)
        {
            string tr = string.Format("{0,-20} {1,-25}\n", "Time", "Trainer");
            tr += new string('-', 103) + "\n";

            List<TrainerUses> trus = MyDS.GetTrUses(cust);

            foreach (TrainerUses t in trus)
            {
                tr += PrintTrainerUse(t);
            }

            return tr;
        }//Valmis

        public string PrintCust(Customer customer)
        {
            string custs = string.Empty;
            if (customer.Membership_ref > 0 && customer.PersonalTrainer == 0)
            {
                Membership m = GetMembShip("MembershipID", customer.Membership_ref.ToString());
                custs += string.Format("{0,-5} {1,-25} {2,-25} {3,-28} {4,-30}\n",
                    customer.ID,
                    $"{customer.FirstName} {customer.LastName}",
                    customer.Email,
                    m.ToString(),
                    "None");
            }
            else if (customer.Membership_ref > 0 && customer.PersonalTrainer > 0)
            {
                Membership m = GetMembShip("MembershipID", customer.Membership_ref.ToString());
                Trainer t = GetTrnr("TrainerID", customer.PersonalTrainer.ToString());
                custs += string.Format("{0,-5} {1,-25} {2,-25} {3,-28} {4,-30}\n",
                    customer.ID,
                    $"{customer.FirstName} {customer.LastName}",
                    customer.Email,
                    m.ToString(),
                    t.ToString());
            }
            else
            {
                custs += string.Format("{0,-5} {1,-25} {2,-25} {3,-28} {4,-30}\n",
                    customer.ID,
                    $"{customer.FirstName} {customer.LastName}",
                    customer.Email,
                    "None",
                    "None");
            }
            return custs;
        }//Valmis

        public string PrintCustInfo(Customer customer)
        {
            string custs = new string('-', 103) + "\n";
            if (customer.Membership_ref > 0)
            {
                Membership m = GetMembShip("MembershipID", customer.Membership_ref.ToString());
                int GV = MyDS.Count("GymVisits", "Membership_ref", m.ID.ToString());
                int CV = MyDS.Count("Attendance", "Membership_ref", m.ID.ToString());
                int TU = MyDS.Count("TrainerUses", "Membership_ref", m.ID.ToString());
                
                custs += string.Format("{0,-20} {1,-20} {2,-20}\n", "Gym visits: ", "Class visits: ", "Trainer uses: (During ongoing membership)");
                custs += string.Format("{0,-20} {1,-20} {2,-20}",
                        GV,
                        CV,
                        TU);
            }
            
            return custs;
        }//Valmis

        public int AttendPerClass(Schedule s)
        {
            int custs = MyDS.Count("Attendance", "Schedule_ref", s.ScheduleID.ToString());

            return custs;
        }

        public string PrintAttendance(Attendance attendance)
        {
            Schedule s = GetSchedule("ScheduleID", attendance.Schedule_ref.ToString());
            Class c = GetClass("ClassID", s.Class_ref.ToString());
            Trainer t = GetTrnr("TrainerID", s.Trainer_ref.ToString());

            string att = string.Empty;
            att += string.Format("{0,-25} {1,-25} {2,-28}\n",
                c.Name,
                s.Time.ToString("dd.MM.yyyy HH:mm"),
                $"{t.FirstName} {t.LastName}");
            return att;
        }//Valmis

        public string PrintTrainerUse(TrainerUses t)
        {
            Trainer trnr = GetTrnr("TrainerID", t.Trainer_ref.ToString());

            string trus = string.Empty;
            trus += string.Format("{0,-30} {1,-25}\n",
                t.VisitTime.ToString("dd.MM.yyyy HH:mm"),
                $"{trnr.FirstName} {trnr.LastName}");
            return trus;
        }//Valmis

        public string FullReport(Customer cust)
        {
            string rep = string.Format("{0,-5} {1,-25} {2,-25} {3,-28} {4,-30}\n", "ID", "Name", "Email", "Membership", "Personaltrainer");
            rep += new string('-', 103) + "\n" + PrintCust(cust) + "\n" + PrintCustInfo(cust) + "\n\n";
            rep += "Gym visits during ongoing membership:\n" + "-------------------------------------\n";
            rep += GetVisits(cust);
            rep += "\nClass attendances during ongoing membership:\n" + "--------------------------------------------\n";
            rep += GetAttendances(cust);
            rep += "\nTrainer appointments during ongoing membership:\n" + "-----------------------------------------------\n";
            rep += GetTrainerUses(cust);

            return rep;
        }//Valmis

        private bool MembValid(Customer customer)
        {
            if (customer.Membership_ref > 0)
            {
                Membership m = GetMembShip("MembershipID", customer.Membership_ref.ToString());
                if ((m.End - DateTime.Now).Days > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else { return false; }
        }//Valmis

        private bool AttendValid(Customer cust)
        {
            Membership m = GetMembShip("Customer_ref", cust.ID.ToString());
            int ClassAttended = MyDS.Count("Attendance", "Membership_ref", m.ID.ToString());

            if(m.Name == "Monthly")
            {
                if(ClassAttended < 10) { return true; }
                else { return false; }
            }
            else if (m.Name == "Half a year")
            {
                if(ClassAttended < 70) { return true; }
                else { return false; }
            }else if(m.Name == "Yearly")
            {
                if(ClassAttended < 160) { return true; }
                else { return false; }
            }
            else return false;
        }//Valmis

        private bool TrainerValid(Customer cust)
        {
            Membership m = GetMembShip("Customer_ref", cust.ID.ToString());
            int TrainerUsed = MyDS.Count("TrainerUses", "Membership_ref", m.ID.ToString());

            if (m.Name == "Monthly")
            {
                if (TrainerUsed < 5) { return true; }
                else { return false; }
            }
            else if (m.Name == "Half a year")
            {
                if (TrainerUsed < 35) { return true; }
                else { return false; }
            }
            else if (m.Name == "Yearly")
            {
                if (TrainerUsed < 80) { return true; }
                else { return false; }
            }
            else return false;
        }//Valmis

        public void UpdateCust(Customer cust)
        {
            string[] fields;
            string[] values;
            int id = cust.ID;
            string fn = cust.FirstName;
            string ln = cust.LastName;
            string email = cust.Email;
            string memb = cust.Membership_ref.ToString();
            string pt = cust.PersonalTrainer.ToString();

            fields = new string[] { "[FirstName]", "[LastName]", "[Email]", "[Membership_ref]", "[Trainer_ref]" };
            values = new string[] { fn, ln, email, memb, pt };
            MyDS.Update("Customer", fields, values, id.ToString());

            
            
        }//Valmis

        public void UpdateTrnr(Trainer trnr)
        {
            string[] fields;
            string[] values;
            int id = trnr.ID;
            string fn = trnr.FirstName;
            string ln = trnr.LastName;
            string email = trnr.Email;


            fields = new string[] { "FirstName", "LastName", "Email" };
            values = new string[] { fn, ln, email };
            MyDS.Update("Trainer", fields, values, id.ToString());

        }//Valmis

        public void UpdateClass(Class c)
        {
            string[] fields;
            string[] values;
            int id = c.ID;
            string nm = c.Name;


            fields = new string[] { "ClassName" };
            values = new string[] { nm };
            MyDS.Update("Class", fields, values, id.ToString());
        }

        public void UpdateSchedule(Schedule s)
        {
            string[] fields;
            string[] values;
            int id = s.ScheduleID;
            string time = s.Time.ToString();
            string cl_ref = s.Class_ref.ToString();
            string trnr_ref = s.Trainer_ref.ToString();


            fields = new string[] { "VisitTime", "Class_ref", "Trainer_ref" };
            values = new string[] { time, cl_ref, trnr_ref };
            MyDS.Update("Schedule", fields, values, id.ToString());
        }

        public void RemovePerson(Person p)
        {
            string type = p.GetType().Name;
            int id = p.ID;
            if(type == "Trainer")
            {
                MyDS.Remove(type, id.ToString());
                MyDS.Remove("Schedule", "Trainer_ref", id.ToString());
            }else
            {
                MyDS.Remove(type, id.ToString());
            }
        }//Valmis

        public void RemoveClass(Class c) 
        {
            int id = c.ID;
            MyDS.Remove("Class", id.ToString());
            MyDS.Remove("Schedule", "Class_ref", id.ToString());
        }//Valmis

        public void RemoveSchedule(Schedule s)
        {
            string[] fields = new string[] { "ScheduleID" };
            string[] values = new string[] { s.ScheduleID.ToString() };
            MyDS.RemoveSchedule(fields, values);
        }//Valmis

    }
}
