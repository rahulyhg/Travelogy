//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DomingoDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class TripStepCost
    {
        public int Id { get; set; }
        public string Destination { get; set; }
        public int TripTemplateStepId { get; set; }
        public string ShortDescription { get; set; }
        public int TripTemplateId { get; set; }
        public string Description { get; set; }
        public string Season { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}