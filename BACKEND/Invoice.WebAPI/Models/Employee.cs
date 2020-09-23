using System;
using System.Collections.Generic;

namespace Invoice.WebAPI.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
