using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{
    public class UserManager
    {
        public int UpdateUserTravellerProfile(Traveller _travellerObj)
        {
            int _status = DomingoDataController.UpdateTraveller(_travellerObj);
            return -1;
        }
    }
}
