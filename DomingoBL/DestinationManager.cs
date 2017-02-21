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
        /// <param name="country"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        public static DomingoBlError GetDestinationsForCountry(string country, out List<Destination> destinations)
        {
            destinations = null;

            try
            {
                // get all desitions that matches the continent, reverse order by weightage
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    destinations = context.Destinations
                        .Where(p => p.Country == country && p.Name != country).OrderByDescending(p => p.Weightage).ToList();
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
        /// <param name="destinations"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static DomingoBlError GetTopDestinations(out List<Destination> destinations, int top)
        {
            destinations = null;

            try
            {
                // get all desitions, reverse order by weightage
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var _allDestinations = context.Destinations.Select(p => p).OrderByDescending(p => p.Weightage).Take(top);
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

        //public static DomingoBlError GetSuggestionsForTrip(int tripId, )
        //{

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="topRecs"></param>
        /// <param name="destinations"></param>
        /// <returns></returns>
        public static DomingoBlError GetTopDestinations(string country, int topRecs, out List<Destination> destinations)
        {
            destinations = null;

            try
            {
                // get all desitions, reverse order by weightage
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var _allDestinations = string.IsNullOrEmpty(country) ?
                            context.Destinations.Select(p => p).OrderByDescending(p => p.Weightage)
                            : context.Destinations.Where(p => p.Country == country).OrderByDescending(p => p.Weightage);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="subDestinationList"></param>
        /// <returns></returns>
        public static DomingoBlError SearchSubDestination(string search, out List<SubDestination> subDestinationList)
        {
            subDestinationList = new List<SubDestination>();

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    subDestinationList.AddRange(context.SubDestinations.Where(p => p.Name == search || p.Type == search).ToList());                    
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
