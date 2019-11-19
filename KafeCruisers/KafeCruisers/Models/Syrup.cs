using System;
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

        public int CaramelSyrup { get; set; }

        public int ChocolateSyrup { get; set; }

        public int PeppermintSyrup { get; set; }

        public int VanillaSyrup { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }
    }
}