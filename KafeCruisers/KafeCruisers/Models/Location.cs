using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Display(Name = "Location")]
        public string LocationName { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        


    }
}