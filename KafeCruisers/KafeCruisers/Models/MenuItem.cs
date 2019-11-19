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


        [ForeignKey("Menu")]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

    }
}