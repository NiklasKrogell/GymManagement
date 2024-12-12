using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSovellus
{
    public class Trainer : Person
    {
        public Trainer(int id, string fn, string ln, string email) : base(id, fn, ln, email)
        {
            // testi kommentointi saa poistaa
        }

        public override string ToString()
        {
            string stringi = $"{FirstName} {LastName}";

            return stringi;
        }
    }
}
