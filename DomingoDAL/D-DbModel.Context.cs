﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TravelogyDevEntities1 : DbContext
    {
        public TravelogyDevEntities1()
            : base("name=TravelogyDevEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Destination> Destinations { get; set; }
        public virtual DbSet<DestinationCountry> DestinationCountries { get; set; }
        public virtual DbSet<DestinationLink> DestinationLinks { get; set; }
        public virtual DbSet<DestinationTravelOption> DestinationTravelOptions { get; set; }
        public virtual DbSet<SubDestination> SubDestinations { get; set; }
        public virtual DbSet<TripProvider> TripProviders { get; set; }
        public virtual DbSet<Traveller> Travellers { get; set; }
        public virtual DbSet<TravellerType> TravellerTypes { get; set; }
        public virtual DbSet<Thread> Threads { get; set; }
        public virtual DbSet<ThreadMessage> ThreadMessages { get; set; }
    }
}
