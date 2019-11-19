using System;
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

        public bool CaramelDrizzle { get; set; }

        public bool ChocolateDrizzle { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}