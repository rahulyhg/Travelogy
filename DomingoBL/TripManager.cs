using DomingoBL.BlObjects;
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
        public static DomingoBlError GetAllTripsForUser(string aspUserId, out List<View_Trip> trips)
        {
            trips = null;

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    trips = context.View_Trip.Where(p => p.AspNetUserId == aspUserId).ToList();
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
                    if (trip.Id == 0)
                    {
                        context.Trips.Add(trip);

                        // get the steps from the template
                        foreach(var tripStep in _CopyTripSteps(trip.TripTemplateId, context))
                        {
                            context.TripSteps.Add(tripStep);
                        }

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

        private static List<TripStep> _CopyTripSteps(int tripTemplateId, TravelogyDevEntities1 context)
        {
            var tripSteps = new List<TripStep>();
            var templateSteps = context.TripTemplateSteps.Where(p => p.TripTemplateId == tripTemplateId);
            foreach(var templateStep in templateSteps)
            {
                var tripStep = new TripStep()
                {
                    ShortDescription = templateStep.ShortDescription,
                    LongDescription = templateStep.LongDescription,
                    NightStay = templateStep.NightStay                     
                };
                tripSteps.Add(tripStep);
            }
            return tripSteps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static DomingoBlError SearchTripTemplatesByAlias(string alias, out List<BlTripTemplate> templates)
        {
            try
            {
                templates = new List<BlTripTemplate>();

                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // search for trmplates with the search alias
                    var dbTemplateSearchRes = context.TripTemplates.Where(p => p.SearchAlias == alias).ToList();

                    // iterate through the results and populate the business layer object
                    foreach(var dbTemplate in dbTemplateSearchRes)
                    {
                        // add the template and then the steps
                        var _blTripTemplateObj = new BlTripTemplate() { DlTemplate = dbTemplate };
                        _blTripTemplateObj.DlTemplateSteps = context.TripTemplateSteps.Where(p => p.TripTemplateId == dbTemplate.Id).ToList();

                        templates.Add(_blTripTemplateObj);
                    }
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
        public static DomingoBlError GetTripTemplatesById(int templateId, out BlTripTemplate template)
        {
            template = new BlTripTemplate();
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // add the template db obj to the bl object
                    template.DlTemplate = context.TripTemplates.Where(p => p.Id == templateId).FirstOrDefault();                    

                    // add the template steps
                    template.DlTemplateSteps = context.TripTemplateSteps.Where(p => p.TripTemplateId == templateId).ToList();                    
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
        /// <param name="tripId"></param>
        /// <param name="trip"></param>
        /// <returns></returns>
        public static DomingoBlError GetTripById(int tripId, out View_Trip trip)
        {
            trip = null;
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    trip = context.View_Trip.Where(p => p.Id == tripId).FirstOrDefault();
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
