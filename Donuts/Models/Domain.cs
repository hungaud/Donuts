using Donuts.Models.Enums;
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

        [Required(ErrorMessage = "required")]
        [MinLength(10)]
        //[RegularExpression(@"^[a-zA-Z.]+$", ErrorMessage = "Use letters only please")]
        public string Name { get; set; }

        public DateTime RegistrationDate { get; set; }

        public Customer Customer { get; set; }

        public int? CustomerId { get; set; }
        
    }
}
