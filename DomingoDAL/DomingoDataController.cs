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
    public class DomingoDataController
    {
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
