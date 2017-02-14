using DomingoBL.EmailManagement;
using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{
    public class DomingoUserManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FIRST_NAME"></param>
        /// <param name="LAST_NAME"></param>
        /// <param name="EMAIL"></param>
        /// <param name="PHONE"></param>
        /// <param name="TRIP_REQUEST"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> CreateCrmLead(string FIRST_NAME, string LAST_NAME, string EMAIL, string PHONE, string TRIP_REQUEST)
        {
            try
            {
                // send a mail to the customer
                var emailUtility = new EmailUtility();
                var emailParams = new Dictionary<String, String>();
                emailParams.Add("UserName", FIRST_NAME);                
                await emailUtility.SendEmail("ContactUs", EMAIL, emailParams);

                // create a lead in the capsule CRM
                var gateway = new CapsupleCrmGateway();
                var crmResponse = await gateway.CreateCapsuleLead(FIRST_NAME, LAST_NAME, EMAIL, PHONE, TRIP_REQUEST);               

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> TraceSession(string userId, string url)
        {
            try
            {
                var session = new AspNetUserSession() { AspNetUserId = userId, ActionTimeStamp = DateTime.Now, ActionUrl = url };
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    context.AspNetUserSessions.Add(session);
                    await context.SaveChangesAsync();
                }

                return new DomingoBlError() { ErrorCode = 0, ErrorMessage = "" };
            }
            catch (Exception ex)
            {
                return new DomingoBlError() { ErrorCode = 100, ErrorMessage = ex.Message };
            }
        }

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
        public static string GetUserType(string AspNetUserName)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var user = context.AspNetUsers.FirstOrDefault(p => p.UserName == AspNetUserName);
                    if (user != null)
                    {
                        return user.UserType;
                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
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
