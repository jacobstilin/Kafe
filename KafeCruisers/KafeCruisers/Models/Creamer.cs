using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class Creamer
    {
        [Key]
        public int CreamerId { get; set; }

        public int OnePercentMilk { get; set; }

        public int TwoPercentMilk { get; set; }

        public int CoconutMilk { get; set; }

        public int Cream { get; set; }

        public int HeavyCream { get; set; }

        public int SkimMilk { get; set; }

        public int SoyMilk { get; set; }

        public int AlmondMilk { get; set; }

        public int WholeMilk { get; set; }

        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; }

    }
}