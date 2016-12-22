using System.Collections.Generic;

namespace DomingoDAL
{
    public interface IDataController
    {
        List<TripProvider> GetAllTripProviders();
    }
}