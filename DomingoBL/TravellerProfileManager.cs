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
    public class TravellerProfileManager 
    {
        public static DomingoBlError UpdateUserTravellerProfile(Traveller _travellerObj)
        {   
            int _status = DomingoDataController.UpdateTraveller(_travellerObj);
            return new DomingoBlError() { ErrorCode = 100, ErrorMessage = "Not Implemented" };
        }

        public static DomingoBlError GetTravellerProfile(string aspnetUserId, out Traveller _travellerObj)
        {
            _travellerObj = null;
            return new DomingoBlError() { ErrorCode = 100, ErrorMessage = "Not Implemented" };
        }
    }
}
