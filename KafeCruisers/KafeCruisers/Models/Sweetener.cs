using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Sweetener
    {
        [Key]
        public int SweeterId { get; set; }

        public int Honey { get; set; }

        public int SugarInTheRaw { get; set; }

        public int Sugar { get; set; }

        public int Splenda { get; set; }

        public int Equal { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }
    }
}