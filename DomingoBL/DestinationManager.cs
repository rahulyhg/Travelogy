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
    public class DestinationManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="continent"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        public static DomingoBlError GetDestinationsForContinent(string continent, out List<Destination> destinations)
        {
            destinations = null;

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    destinations = context.Destinations.Where(p => p.TourContinent == continent).ToList();                    
                }
            }

            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="destinations"></param>
        /// <returns></returns>
        public static DomingoBlError GetAllDestinations(out List<Destination> destinations)
        {
            destinations = null;

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var _allDestinations = from _destination in context.Destinations select _destination;
                    if (_allDestinations != null)
                    {
                        destinations = _allDestinations.ToList();
                    }
                }
            }

            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }

            return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
        }
    }
}
