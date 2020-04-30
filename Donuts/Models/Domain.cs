using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Donuts.Models
{
    public class Domain
    {

        public int DomainId { get; set; }

        public DateTime ExperiationDate { get; set; }

        [MinLength(10, ErrorMessage = "Domain name must be at least 10 characters long")]
        public string Name { get; set; }

        public DateTime RegistrationDate { get; set; }

        public User User { get; set; }

        public int? UserId { get; set; }

    }
}
