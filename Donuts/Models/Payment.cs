﻿using Donuts.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Donuts.Models
{
    public class Payment
    {
        public decimal Amount { get; set; }

        public decimal AmountPaid { get; set; }

        public DateTime? DueDate { get; set; }

        public Customer Customer { get; set; }

        public int CustomerId { get; set; }

        public Domain Domain { get; set; }

        public int DomainId { get; set; }

        public int PaymentId { get; set; }

        public PaymentStatus Status { get; set; }
    }
}
