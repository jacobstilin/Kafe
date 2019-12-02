using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Display(Name = "ID")]
        public int? UniqueId { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Fill Time")]
        public DateTime FillTime { get; set; }

        [Display(Name = "Price")]
        public double? OrderPrice { get; set;
        }
        [ForeignKey("Truck")]
        public int? TruckId { get; set; }
        public Truck Truck { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        
        public int? Duration { get; set; }
    }
}