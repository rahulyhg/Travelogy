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
        /// <returns></returns>
        public static DomingoBlError CreateTrip()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }
    }
}
