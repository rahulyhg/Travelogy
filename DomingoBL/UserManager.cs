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
        /// <summary>
        /// Check if the user is an admin
        /// </summary>
        /// <param name="AspNetUserName"></param>
        /// <returns></returns>
        public static bool IsTravelogyAdmin(string AspNetUserName)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var admin = context.AspNetUsers.FirstOrDefault(p => p.UserName == AspNetUserName && p.UserType.Trim() == "admin");
                    if(admin != null)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AspNetUserName"></param>
        /// <returns></returns>
        public static bool IsUserEmailVerified(string AspNetUserName)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var user = context.AspNetUsers.FirstOrDefault(p => p.UserName == AspNetUserName && p.EmailConfirmed == true);
                    if (user != null)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
