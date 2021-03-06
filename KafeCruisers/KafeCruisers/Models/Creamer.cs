﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Creamer
    {
        [Key]
        public int CreamerId { get; set; }

        [Display(Name = "Creamer Type")]
        public string CreamerType { get; set; }

        public int? Splashes { get; set; }

        public double? Price { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }

    }
}