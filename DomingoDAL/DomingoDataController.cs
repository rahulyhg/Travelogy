using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoDAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DomingoDataController : IDataController
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public List<Destination> GetAllDestinations()
        //{
        //    using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
        //    {
        //        var _allDestinations = from _destination in context.Destinations
        //                                                select _destination;
        //        if(_allDestinations != null)
        //        {
        //            return _allDestinations.ToList();
        //        }
        //    }

        //    return null;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<TripProvider> GetAllTripProviders()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Country> GetAllCountries()
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var _allCountries = from _country in context.Countries.OrderBy(p => p.Id) select _country;

                    if (_allCountries != null)
                    {
                        return _allCountries.ToList();
                    }
                }

                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }            

    }
}
