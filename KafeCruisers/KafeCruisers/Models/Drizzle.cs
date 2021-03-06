﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Drizzle
    {
        [Key]
        public int DrizzleId { get; set; }

        [Display(Name = "Drizzle Type")]
        public string DrizzleType { get; set; }

        public int? Drizzles { get; set; }

        public double? Price { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}