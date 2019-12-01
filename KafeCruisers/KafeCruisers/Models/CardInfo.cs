using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KafeCruisers.Models
{
    public class CardInfo
    {
        [Key]
        public int CardId { get; set; }
        [Display(Name ="Card Name")]
        public string CardName { get; set; }
        public string Email { get; set; }
        public string StripeToken { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}