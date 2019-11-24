using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }

        public string SizeName { get; set; }

        public double? AdditionalCost { get; set; }

        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; }
    }
}