using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSovellus
{
    public abstract class Person
    {
        public int ID { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Person(int id, string fn, string ln, string email)
        {
            ID = id;
            FirstName = fn;
            LastName = ln;
            Email = email;
        }

    }
}
