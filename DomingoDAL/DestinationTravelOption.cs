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
    
    public partial class DestinationTravelOption
    {
        public int Id { get; set; }
        public int DestinationId { get; set; }
        public string TravelType { get; set; }
        public string TravelMode { get; set; }
        public string TripSource { get; set; }
        public Nullable<decimal> IndicativeCost { get; set; }
    }
}
