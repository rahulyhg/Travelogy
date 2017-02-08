using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL.BlObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class BlTrip
    {
        /// <summary>
        /// 
        /// </summary>
        public Trip DlTrip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<TripStep> DlTripSteps { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<View_TripBookingAccommodation> DlBookingsView { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<View_TripBookingTransport> DlTransportsBookingsView { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BlViewTrip
    {
        /// <summary>
        /// 
        /// </summary>
        public View_Trip DlTripView { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<View_TripStep> DlTripStepsView { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<View_TripBookingAccommodation> DlBookingsView { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<View_TripBookingTransport> DlTransportsBookingsView { get; set; }
    }
}
