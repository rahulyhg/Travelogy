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
    
    public partial class AirportConnection
    {
        public int Id { get; set; }
        public int SourceId { get; set; }
        public int DestinationId { get; set; }
        public Nullable<decimal> FlightTime1 { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string FlightNumber { get; set; }
        public Nullable<int> Stops { get; set; }
    }
}
