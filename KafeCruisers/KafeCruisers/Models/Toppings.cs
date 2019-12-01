using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Toppings
    {
        [Key]
        public int ToppingsId { get; set; }

        [Display(Name = "Toppings Type")]
        public string ToppingsType { get; set; }

        [Display(Name = "Amount")]
        public int? ToppingsAmmount { get; set; }

        public double? Price { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }


    }
}