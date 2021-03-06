﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Syrup
    {
        [Key]
        public int SyrupId { get; set; }

        [Display(Name = "Syrup Type")]
        public string SyrupType { get; set; }

        [Display(Name = "Pumps")]
        public int? SyrupPumps { get; set; }

        public double? Price { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }
    }
}