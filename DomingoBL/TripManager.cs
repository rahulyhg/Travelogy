using DomingoDAL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{

    /// <summary>
    /// 
    /// </summary>
    public enum TripStatus
    {
        planned,

        booked,

        archived
    }

    /// <summary>
    /// 
    /// </summary>
    public class TripManager
    {

        //public static DomingoBlError GetCurrentTripForUser(string aspUserId, out Trip currentTrip)
        //{ }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspUserId"></param>
        /// <param name="trips"></param>
        /// <returns></returns>
        public static DomingoBlError GetAllTripsForUser(string aspUserId, out List<Trip> trips)
        {
            trips = null;

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    trips = context.Trips.Where(p => p.AspNetUserId == aspUserId).ToList();
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trip"></param>
        /// <returns></returns>
        public static DomingoBlError CreateTrip(Trip trip)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // if the trip object is for a new trip
                    // simply add it to the database table and save it
                    if(trip.Id == 0)
                    {
                        context.Trips.Add(trip);
                        context.SaveChanges();
                    }
                    
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static DomingoBlError SearchTripTemplatesByAlias(string alias, out List<TripTemplate> templates)
        {
            try
            {
                templates = null;

                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    templates = context.TripTemplates.Where(p => p.searchalias == alias).ToList();
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                templates = null;
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static DomingoBlError GetTripTemplatesById(int templateId, out TripTemplate template)
        {
            template = null;
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    template = context.TripTemplates.Where(p => p.Id == templateId).FirstOrDefault();
                }
                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" }; 
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }
    }
}
