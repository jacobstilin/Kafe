using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Sauce
    {

        [Key]
        public int SauceId { get; set; }

        [Display(Name = "Sauce Type")]
        public string SauceType { get; set; }

        [Display(Name = "Pumps")]
        public int? SaucePumps { get; set; }

        public double? Price { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }
    }
}