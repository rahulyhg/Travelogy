using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoDAL
{
    public class DomingoDataController : IDataController
    {
        public List<Destination> GetAllDestinations()
        {
            using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
            {
                var _allDestinations = from _destination in context.Destinations
                                                        select _destination;
                if(_allDestinations != null)
                {
                    return _allDestinations.ToList();
                }
            }

            return null;
        }

        public List<TripProvider> GetAllTripProviders()
        {
            throw new NotImplementedException();
        }

        public static int UpdateTraveller (Traveller _travellerObj)
        {
            try
            {
                return 0;
            }
            catch (Exception)
            {
                throw new DomingoDalException();
            }
            
        }

    }
}
