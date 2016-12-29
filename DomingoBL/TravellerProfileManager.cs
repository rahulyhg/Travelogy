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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_travellerObj"></param>
        /// <returns></returns>
        public static DomingoBlError UpdateUserTravellerProfile(Traveller _travellerObj)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // look for the profile
                    var _xtravellerObj = context.Travellers.FirstOrDefault(p => p.AspnetUserid == _travellerObj.AspnetUserid);

                    // if it exists, update it
                    if(_xtravellerObj != null)
                    {
                        _xtravellerObj.FirstName = _travellerObj.FirstName;
                        _xtravellerObj.LastName = _travellerObj.LastName;                        
                    }
                                        
                    else // create it
                    {
                        context.Travellers.Add(_travellerObj);
                    }

                    // save everything
                    context.SaveChanges();
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
        /// <param name="aspnetUserId"></param>
        /// <param name="_travellerObj"></param>
        /// <returns></returns>
        public static DomingoBlError GetTravellerProfile(string aspnetUserId, out Traveller _travellerObj)
        {
            _travellerObj = null;

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    _travellerObj = context.Travellers.FirstOrDefault(p => p.AspnetUserid == aspnetUserId);
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
