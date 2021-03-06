﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string lastName { get; set; }

        public int? OrderTruckId { get; set; }

        public int? CurrentOrderId { get; set; }

        public int? CurrentOrderItemId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public bool IsStripeSetUp { get; set; }
        
        public string CustomerStripeId { get; set; }
    }
}