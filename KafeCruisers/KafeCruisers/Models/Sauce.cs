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

        public int MochaSauce { get; set; }

        public int CaramelSauce { get; set; }

        public int PumpkinSauce { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }
    }
}