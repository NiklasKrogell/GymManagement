using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSovellus
{
    public class Customer : Person
    {
        DataService MyDS = DataService.GetDataService();
        public int Membership_ref { get; set; }
        public int PersonalTrainer { get; set; }

        public Customer(int id, string fn, string ln, string email) : base(id, fn, ln, email)
        {
        }

        public Customer(int id, string fn, string ln, string email, string memb) : this(id, fn, ln, email)
        {
            Membership_ref = Convert.ToInt32(memb);
        }

        public Customer(int id, string fn, string ln, string email, string Memb, string pt) : this(id, fn, ln, email, Memb)
        {
            PersonalTrainer = Convert.ToInt32(pt);
        }

        public override string ToString()
        {
            string s = FirstName + " " + LastName;

            return s;
        }

    }
}
