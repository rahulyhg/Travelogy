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
                // get all desitions that matches the continent, reverse order by weightage
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    destinations = context.Destinations.Where(p => p.TourContinent == continent).OrderByDescending(p => p.Weightage).ToList();                    
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
                // get all desitions, reverse order by weightage
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var _allDestinations = context.Destinations.Select(p => p).OrderByDescending(p => p.Weightage);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="continent"></param>
        /// <param name="topRecs"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        public static DomingoBlError GetTopDestinations(string continent, int topRecs, out List<Destination> destinations)
        {
            destinations = null;

            try
            {
                // get all desitions, reverse order by weightage
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var _allDestinations = string.IsNullOrEmpty(continent) ?
                            context.Destinations.Select(p => p).OrderByDescending(p => p.Weightage)
                            : context.Destinations.Where(p => p.TourContinent == continent).OrderByDescending(p => p.Weightage);

                    if (_allDestinations != null)
                    {
                        if(_allDestinations.Count() > topRecs)
                        {
                            destinations = _allDestinations.Take(topRecs).ToList();
                        }

                        else
                        {
                            destinations = _allDestinations.ToList();
                        }
                        
                    }
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
        /// <param name="alias"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        public static DomingoBlError GetDestinationForAlias(string alias, out Destination destination)
        {
            destination = null;

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    destination = context.Destinations.Where(p => p.Alias == alias).FirstOrDefault();
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
