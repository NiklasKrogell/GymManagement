using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;
using System.Collections;

namespace GymSovellus
{
    internal class DataService
    {
        static DataService? MyDS;
        private OleDbConnection myConnection;
        private OleDbDataReader myReader;
        

        private DataService() 
        {
            string connstr;
            string projectPath = @"..\..\..\..\GymGraafinen\Data";
            connstr = "Provider = Microsoft.ACE.OLEDB.12.0;" + @"Data Source = " +
            projectPath + @"\GymData.accdb;";
            myConnection = new OleDbConnection();
            myConnection.ConnectionString = connstr;
            myConnection.Open();
        }

        public OleDbDataReader GetData(string[] fields, string table)
        {
            OleDbCommand myCommand = new OleDbCommand();

            myCommand.Connection = myConnection;

            myCommand.CommandText = "SELECT ";

            foreach (string s in fields)
                myCommand.CommandText += s + ", ";

            myCommand.CommandText = myCommand.CommandText.Remove(myCommand.CommandText.LastIndexOf(","));
            myCommand.CommandText += " FROM " + table;

            myCommand.CommandType = CommandType.Text;

            OleDbDataReader myReader;
            myReader = myCommand.ExecuteReader();

            
            return myReader;
        }

        public Dictionary<string, object> SelectWhere(string table, string field, string value)
        {
            OleDbCommand myCommand = new OleDbCommand();

            myCommand.Connection = myConnection;

            myCommand.CommandText = $"SELECT * FROM {table} WHERE {field} = {value};";

            myCommand.CommandType = CommandType.Text;

            Dictionary<string, object> data = new Dictionary<string, object>();

            OleDbDataReader myReader;
            myReader = myCommand.ExecuteReader();

            if (myReader.Read())
            {
                for(int i= 0; i< myReader.FieldCount; i++)
                {
                    string otsikko = myReader.GetName(i);
                    object arvo = myReader.GetValue(i);
                    data[otsikko] = arvo;
                }

                return data;
            }
            else
            {
                throw new Exception("Data not found");
            }           
        }

        public Membership GetMembershipByID(int id)
        {
            OleDbCommand myCommand = new OleDbCommand();

            myCommand.Connection = myConnection;

            myCommand.CommandText = $"SELECT * FROM Membership WHERE Membership.MembershipID = {id};";

            myCommand.CommandType = CommandType.Text;

            Membership m;


            OleDbDataReader myReader;
            myReader = myCommand.ExecuteReader();

            if (myReader.Read())
            {
                int membId = Convert.ToInt32(myReader["MembershipID"]);
                string name = myReader["MembName"].ToString();
                DateTime start = Convert.ToDateTime(myReader["StartDate"]);
                DateTime end = Convert.ToDateTime(myReader["EndDate"]);
                int custid = Convert.ToInt32(myReader["Customer_ref"]);

                m = new Membership(membId, name, start, end, custid);
                return m;
            }
            else 
            {
                throw new Exception("Membership not found");
            }
        }

        public List<Customer> GetAllCustomers() 
        {
            string[] field =new string[] { "*" };

            OleDbDataReader myReader = GetData(field, "Customer");

            List<Customer> customers = new List<Customer>();

            bool notEoF;

            notEoF = myReader.Read();

            while (notEoF)
            {
                int id = Convert.ToInt32(myReader["CustomerID"].ToString());
                string fn = myReader["FirstName"].ToString();
                string ln = myReader["LastName"].ToString();
                string em = myReader["Email"].ToString();
                string? Memb = myReader["Membership_ref"].ToString();
                string? Pt = myReader["Trainer_ref"].ToString();

                customers.Add(new Customer(id, fn, ln, em, Memb, Pt));

                notEoF = myReader.Read();
            }

            return customers;
        }

        public List<Trainer> GetAllTrainers()
        {
            string[] field = new string[] { "*" };

            OleDbDataReader myReader = GetData(field, "Trainer");

            List<Trainer> trainers = new List<Trainer>();

            bool notEoF;

            notEoF = myReader.Read();

            while (notEoF)
            {
                int id = Convert.ToInt32(myReader["TrainerID"].ToString());
                string fn = myReader["FirstName"].ToString();
                string ln = myReader["LastName"].ToString();
                string em = myReader["Email"].ToString();

                trainers.Add(new Trainer(id, fn, ln, em));

                notEoF = myReader.Read();
            }

            return trainers;
        }

        public List<Schedule> GetAllSchedules()
        {
            string[] field = new string[] { "*" };

            OleDbDataReader myReader = GetData(field, "Schedule");

            List<Schedule> schedules = new List<Schedule>();

            bool notEoF;

            notEoF = myReader.Read();

            while (notEoF)
            {
                int id = Convert.ToInt32(myReader["ScheduleID"].ToString());
                DateTime time = Convert.ToDateTime(myReader["VisitTime"]);
                int Clas = Convert.ToInt32(myReader["Class_ref"]);
                int pt = Convert.ToInt32(myReader["Trainer_ref"]);

                schedules.Add(new Schedule(id, Clas, pt, time));

                notEoF = myReader.Read();
            }

            return schedules;
        }

        public List<Attendance> GetAttendances(Customer cust)
        {
            OleDbCommand myCommand = new OleDbCommand();

            Membership m = GetMembershipByID(cust.Membership_ref);

            string st = m.Start.ToString("yyyy-MM-dd HH:mm:ss");
            string en = m.End.ToString("yyyy-MM-dd HH:mm:ss");

            myCommand.Connection = myConnection;

            myCommand.CommandText = @$"SELECT Attendance.Membership_ref, Attendance.Schedule_ref
                                        FROM Attendance INNER JOIN Schedule
                                        ON Attendance.Schedule_ref = Schedule.ScheduleID
                                        WHERE Attendance.Membership_ref = {cust.Membership_ref} 
                                        AND Schedule.[VisitTime] BETWEEN #{st}# AND #{en}#;";

            myCommand.CommandType = CommandType.Text;

            List<Attendance> data = new List<Attendance>();

            OleDbDataReader myReader;
            myReader = myCommand.ExecuteReader();

            bool notEoF;

            notEoF = myReader.Read();

            while (notEoF)
            {
                int custid = Convert.ToInt32(myReader["Membership_ref"].ToString());
                int schid = Convert.ToInt32(myReader["Schedule_ref"].ToString());

                data.Add(new Attendance(custid, schid));

                notEoF = myReader.Read();
            }

            return data;
        }

        public List<GymVisit> GetVisits(Customer cust)
        {
            Membership m = GetMembershipByID(cust.Membership_ref);
            OleDbCommand myCommand = new OleDbCommand();

            string st = m.Start.ToString("yyyy-MM-dd HH:mm:ss");
            string en = m.End.ToString("yyyy-MM-dd HH:mm:ss");

            myCommand.Connection = myConnection;

            myCommand.CommandText = $"SELECT * FROM GymVisits WHERE Membership_ref = {cust.Membership_ref} AND [VisitTime] BETWEEN #{st}# AND #{en}#;";

            myCommand.CommandType = CommandType.Text;

            List<GymVisit> data = new List<GymVisit>();

            OleDbDataReader myReader;
            myReader = myCommand.ExecuteReader();

            bool notEoF;

            notEoF = myReader.Read();

            while (notEoF)
            {
                int id = Convert.ToInt32(myReader["VisitID"].ToString());
                int custid = Convert.ToInt32(myReader["Membership_ref"]);
                DateTime time = Convert.ToDateTime(myReader["VisitTime"]);

                data.Add(new GymVisit(id, custid, time));

                notEoF = myReader.Read();
            }

            return data;
        }

        public List<Class> GetAllClasses()
        {
            string[] field = new string[] { "*" };

            OleDbDataReader myReader = GetData(field, "Class");

            List<Class> classes = new List<Class>();

            bool notEoF;

            notEoF = myReader.Read();

            while (notEoF)
            {
                int id = Convert.ToInt32(myReader["ClassID"].ToString());
                string name = myReader["ClassName"].ToString();


                classes.Add(new Class(id, name));

                notEoF = myReader.Read();
            }

            return classes;
        }//Valmis

        public List<TrainerUses> GetTrUses(Customer cust)
        {
            Membership m = GetMembershipByID(cust.Membership_ref);

            DateTime s = m.Start;
            DateTime e = m.End;

            string start = s.ToString("yyyy-MM-dd HH:mm:ss");
            string end = e.ToString("yyyy-MM-dd HH:mm:ss");

            OleDbCommand myCommand = new OleDbCommand();

            myCommand.Connection = myConnection;

            myCommand.CommandText = $"SELECT * FROM TrainerUses WHERE TrainerUses.Membership_ref = {cust.Membership_ref} AND TrainerUses.[VisitTime] BETWEEN #{start}# AND #{end}#;";

            myCommand.CommandType = CommandType.Text;

            OleDbDataReader myReader;
            myReader = myCommand.ExecuteReader();

            List<TrainerUses> trus = new List<TrainerUses>();

            bool notEoF;

            notEoF = myReader.Read();

            while (notEoF)
            {
                int custid = Convert.ToInt32(myReader["Membership_ref"].ToString());
                DateTime time = Convert.ToDateTime(myReader["VisitTime"]);
                int trnrid = Convert.ToInt32(myReader["Trainer_ref"].ToString());

                trus.Add(new TrainerUses(custid, time, trnrid));

                notEoF = myReader.Read();
            }

            return trus;
        }

        public void InsertInto(string table, string[] fields, string[] values)
        {
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = myConnection;



            myCommand.CommandText = $"INSERT INTO {table} (";
            foreach (string s in fields)
                myCommand.CommandText += s + ", ";

            myCommand.CommandText = myCommand.CommandText.Remove(myCommand.CommandText.LastIndexOf(","));
            myCommand.CommandText += ") VALUES (";
            foreach (string s in values)
                myCommand.CommandText += "?" + ", ";

            myCommand.CommandText = myCommand.CommandText.Remove(myCommand.CommandText.LastIndexOf(","));
            myCommand.CommandText += ")";

            foreach (string s in values)
                myCommand.Parameters.AddWithValue("?", s);        

            myCommand.ExecuteNonQuery();
        }

        public void Update(string tab, string[] fields, string[] values, string id)
        {
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = myConnection;



            myCommand.CommandText = $"UPDATE {tab} SET ";

            for (int i = 0; i < (fields.Length); i++)
            {
                string parameterName = $"@value{i}";
                myCommand.CommandText += $"{fields[i]} = {parameterName}, ";
                myCommand.Parameters.AddWithValue(parameterName, values[i]);
            }
                

            myCommand.CommandText = myCommand.CommandText.Remove(myCommand.CommandText.LastIndexOf(","));
            myCommand.CommandText += $" WHERE {tab}ID = " + id;

            myCommand.CommandType = CommandType.Text;


            myCommand.ExecuteNonQuery();
        }

        public void Remove(string tab, string value)
        {
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = myConnection;


            myCommand.CommandText = $"DELETE * FROM {tab} WHERE {tab}ID = " + value;

            myCommand.CommandType = CommandType.Text;


            myCommand.ExecuteNonQuery();
        }

        public void Remove(string tab, string field, string value)
        {
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = myConnection;


            myCommand.CommandText = $"DELETE * FROM {tab} WHERE {field} = " + value;

            myCommand.CommandType = CommandType.Text;


            myCommand.ExecuteNonQuery();
        }

        public void RemoveSchedule( string[] fields, string[] values)
        {
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = myConnection;



            myCommand.CommandText = $"DELETE * FROM Schedule WHERE ";
            for(int i= 0; i < fields.Length; i++)
            {
                myCommand.CommandText += $"{fields[i]} = {values[i]} AND ";
            }

            myCommand.CommandText = myCommand.CommandText.Substring(0, myCommand.CommandText.LastIndexOf("AND")).TrimEnd();

            myCommand.CommandType = CommandType.Text;


            myCommand.ExecuteNonQuery();
        }

        public int Count(string table, string field, string value) 
        {
            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection= myConnection;
                     
            myCommand.CommandText = $"SELECT SUM(IIF({field} = {value}, 1, 0)) AS VisitCount FROM {table}";

            object result = myCommand.ExecuteScalar();
            
            if (result == DBNull.Value)
            {
                return 0;
            }

            return Convert.ToInt32(result);            
        }

        public static DataService GetDataService()
        {
            if(MyDS == null)
            {
                MyDS = new DataService();
            }
            return MyDS;
        }

        internal void AddMembShip(Membership m, string[] fields, string[] values)
        { 

            OleDbCommand myCommand = new OleDbCommand();
            myCommand.Connection = myConnection;

            myCommand.CommandText = "INSERT INTO Membership (";
            foreach (string s in fields)
                myCommand.CommandText += s + ", ";

            myCommand.CommandText = myCommand.CommandText.Remove(myCommand.CommandText.LastIndexOf(","));
            myCommand.CommandText += ") VALUES (";
            foreach (string s in values)
                myCommand.CommandText += "?" + ", ";

            myCommand.CommandText = myCommand.CommandText.Remove(myCommand.CommandText.LastIndexOf(","));
            myCommand.CommandText += ")";

            foreach (string s in values)
                myCommand.Parameters.AddWithValue("?", s);

            myCommand.ExecuteNonQuery();

            myCommand.CommandText = "SELECT @@IDENTITY";
            int membershipID = Convert.ToInt32(myCommand.ExecuteScalar());

            string[] field = new string[] { "Membership_ref" };
            string[] value = new string[] { membershipID.ToString() };
            Update("Customer", field, value, m.Customer_ref.ToString());
        }
    }
}
