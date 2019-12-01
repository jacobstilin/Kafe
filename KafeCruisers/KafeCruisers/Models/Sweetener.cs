using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Sweetener
    {
        [Key]
        public int SweeterId { get; set; }

        [Display(Name = "Sweetener Type")]
        public string SweetenerType { get; set; }

        public int? Scoops { get; set; }

        public double? Price { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }
    }
}