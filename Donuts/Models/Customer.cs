using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Donuts.Models
{
    public class Customer
    {
        [CreditCard]
        public string CardNumber { get; set; }

        public string ContactId { get; set; }

        public ICollection<Domain> Domains { get; set; }

        public string Password { get; set; }

        public int CustomerId { get; set; }

        //[Index(IsUnique = true)]
        public string CustomerName { get; set; }

        public string ProviderName { get; set; }

        public PublicKey PublicKey { get; set; }

    }
}
