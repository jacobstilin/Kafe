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

        public string TruckName { get; set; }

        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location Location { get; set; }

        public int? StartTime { get; set; }

        public int? EndTime { get; set; }
    }
}