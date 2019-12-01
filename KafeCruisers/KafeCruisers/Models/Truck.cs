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

        [Display(Name = "Truck Name")]
        public string TruckName { get; set; }

        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location Location { get; set; }

        [Display(Name = "Open Time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Close Time")]
        public DateTime EndTime { get; set; }
    }
}