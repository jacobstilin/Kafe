using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Truck
    {
        [Key]
        public int TruckId { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public Location location { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }
    }
}