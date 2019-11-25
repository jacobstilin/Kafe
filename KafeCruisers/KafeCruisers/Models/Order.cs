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

        public int? UniqueId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? FillTime { get; set; }

        public double? OrderPrice { get; set;
        }
        [ForeignKey("Truck")]
        public int? TruckId { get; set; }
        public Truck Truck { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        

    }
}