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
    
    public partial class TripBookingTransport
    {
        public int Id { get; set; }
        public string TransportType { get; set; }
        public decimal EstimatedCost { get; set; }
        public int TripId { get; set; }
        public Nullable<int> TripStepId { get; set; }
    }
}