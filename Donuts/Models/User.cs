using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Donuts.Models
{
    public class User
    {

        public ICollection<Domain> Domains { get; set; }

        public string Password { get; set; }

        public int UserId { get; set; }

        //[Index(IsUnique = true)]
        public string UserName { get; set; }

    }
}
