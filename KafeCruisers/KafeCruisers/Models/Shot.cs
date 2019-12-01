using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Shot
    {
        [Key]
        public int ShotId { get; set; }

        [Display(Name = "Shot Type")]
        public string ShotType { get; set; }

        public int? Shots { get; set; }

        public double? Price { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }

    }
}