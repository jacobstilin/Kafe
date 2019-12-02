using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class OrderItem
    {
        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public Order Order { get; set; }

        [Key]
        public int OrderItemId { get; set; }

        public int IdFromMenu { get; set; }

        public string ItemName { get; set; }

        public string Size { get; set; }

        public double? SizePrice { get; set; }

        public bool Room { get; set; }

        public bool Decaf { get; set; }

        public string Temperature { get; set; }

        public bool WhippedCream { get; set; }

        public double? Price { get; set; }

        public int? Duration { get; set; }

        

        public ICollection<Creamer> Creamers { get; set; }

        public ICollection<Sweetener> Sweeteners { get; set; }

        public ICollection<Shot> Shots { get; set; }

        public ICollection<Sauce> Sauces { get; set; }

        public ICollection<Syrup> Syrups { get; set; }

        public ICollection<Toppings> Toppings { get; set; }

        public ICollection<Powder> Powders { get; set; }

        public ICollection<Drizzle> Drizzles { get; set; }

        




    }
}