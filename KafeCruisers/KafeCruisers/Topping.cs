//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KafeCruisers
{
    using System;
    using System.Collections.Generic;
    
    public partial class Topping
    {
        public int ToppingsId { get; set; }
        public bool PumpkinSpice { get; set; }
        public bool SeaSalt { get; set; }
        public int OrderItemId { get; set; }
    
        public virtual OrderItem OrderItem { get; set; }
    }
}
