using System.Collections.Generic;

namespace Donuts.Models
{
    public class Customer
    {

        public ICollection<Domain> Domains { get; set; }

        public string Password { get; set; }

        public int CustomerId { get; set; }

        //[Index(IsUnique = true)]
        public string CustomerName { get; set; }

    }
}
