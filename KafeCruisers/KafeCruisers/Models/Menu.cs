using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }

        [ForeignKey("Truck")]
        public int? TruckId { get; set; }
        public Truck truck { get; set; }
    }
}