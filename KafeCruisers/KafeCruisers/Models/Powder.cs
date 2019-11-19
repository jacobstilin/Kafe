using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Powder
    {
        [Key]
        public int PowderId { get; set; }

        public bool ChocolatePowder { get; set; }

        public bool CinnamonPowder { get; set; }

        public bool VanillaPoweer { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }
    }
}