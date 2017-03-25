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
        /// <param name="emailAddress"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> CreateCrmLeadWebSignup(string emailAddress, string userId)
        {
            try
            {
                // create a lead in the capsule CRM
                var gateway = new CapsupleCrmGateway();
                var crmResponse = await gateway.CreateCapsuleLead("Websignup", String.Format("{0}",DateTime.Now.Ticks), emailAddress, "Un-known", "Portal Signup");

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
        /// <param name="name"></param>
        /// <param name="telephone"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> CreateCrmLeadCallMeBack(string name, string telephone)
        {
            try
            {
                // create a lead in the capsule CRM
                string fName, lName;
                var names = name.Split(' '); // in case there are two names

                if (names.Length == 2)
                {
                    fName = names[0];
                    lName = names[1];
                }
                else
                {
                    fName = name; lName = "";
                }

                var gateway = new CapsupleCrmGateway();
                var crmResponse = await gateway.CreateCapsuleLead(fName, lName,
                    "Not Captured", telephone, string.Format("{0} {1} clicked CallMeBack on website. Call back number is {2}", fName, lName, telephone));

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
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> CreateCrmLeadDownloadBrochure(string name, string email, string attachment)
        {
            try
            {
                // create a lead in the capsule CRM
                string fName, lName;
                var names = name.Split(' '); // in case there are two names

                if (names.Length == 2)
                {
                    fName = names[0];
                    lName = names[1];
                }
                else
                {
                    fName = name; lName = "";
                }

                var gateway = new CapsupleCrmGateway();
                var crmResponse = await gateway.CreateCapsuleLead(fName, lName, email,
                    "Not Captured", string.Format("{0} {1} clicked Download Brochure on website.", fName, lName));

                var emailUtility = new EmailUtility();
                var emailParams = new Dictionary<String, String>();
                emailParams.Add("UserName", fName);                
                await emailUtility.SendEmailWithAttachment("DownloadBrochure", email, emailParams, attachment);

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
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="telephone"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> CreateCrmLeadUserCallin(string firstName, string lastName, string emailAddress, string telephone, string note)
        {
            try
            {
                // create a lead in the capsule CRM
                var gateway = new CapsupleCrmGateway();
                var crmResponse = await gateway.CreateCapsuleLead(firstName, lastName, emailAddress, telephone, note);

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
        /// <param name="FIRST_NAME"></param>
        /// <param name="LAST_NAME"></param>
        /// <param name="EMAIL"></param>
        /// <param name="PHONE"></param>
        /// <param name="TRIP_REQUEST"></param>
        /// <returns></returns>
        public static async Task<DomingoBlError> CreateCrmLeadExternal(string FIRST_NAME, string LAST_NAME, string EMAIL, string PHONE, string TRIP_REQUEST)
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
