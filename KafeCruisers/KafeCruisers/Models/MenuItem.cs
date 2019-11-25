using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class MenuItem
    {
        [Key]
        public int MenuItemId { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public int? Quantity { get; set; }

        public double? Price { get; set; }

        public ICollection<Size> Sizes { get; set; }

        public ICollection<Temperature> Temperatures { get; set; }

        [ForeignKey("Menu")]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

    }
}