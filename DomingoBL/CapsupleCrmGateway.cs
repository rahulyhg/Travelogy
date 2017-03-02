using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DomingoBL.BlObjects;

namespace DomingoBL
{
    public class CapsupleCrmGateway
    {

        // API key generated on the portal
        internal static readonly string apkiV2Key = "1c2becf937944c73551fab0beeaeaa90";

        //Adding a new record - Use the POST HTTP method to create new records.
        internal static readonly string apiV2CreateNewLead = "https://api.capsulecrm.com/api/v2/parties";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FIRST_NAME"></param>
        /// <param name="LAST_NAME"></param>
        /// <param name="EMAIL"></param>
        /// <param name="PHONE"></param>
        /// <param name="TRIP_REQUEST"></param>
        /// <returns></returns>
        public void CreateCapsuleParty(string FIRST_NAME, string LAST_NAME, string EMAIL, string PHONE, string TRIP_REQUEST)
        {
            // create the capsule CRM object
            var crmParty = new CapsuleCrmParty();
            crmParty.firstName = FIRST_NAME;
            crmParty.lastName = LAST_NAME;
            crmParty.phoneNumbers = new phone_number[] { new phone_number { type = "Home", number = PHONE } };
            crmParty.emailAddresses = new email_address[] { new email_address { type = "Home", address = EMAIL } };

            // get the string payload
            string payLoad = JsonConvert.SerializeObject(crmParty);
            string response = _SendJsonRequestToCapsule(payLoad, apiV2CreateNewLead); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payLoad"></param>
        /// <param name="capsuleApiUrl"></param>
        /// <returns></returns>
        private static string _SendJsonRequestToCapsule(string payLoad, string capsuleApiUrl)
        {
            string returnJson = string.Empty;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(capsuleApiUrl);
            objRequest.Method = "POST";
            objRequest.ContentType = "application/json";
            objRequest.Accept = "application/json";

            using (var streamWriter = new StreamWriter(objRequest.GetRequestStream()))
            {
                streamWriter.Write(payLoad);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)objRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                returnJson = streamReader.ReadToEnd();
            }

            return returnJson;
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
        public async Task<string> CreateCapsuleLead(string FIRST_NAME, string LAST_NAME, string EMAIL, string PHONE, string TRIP_REQUEST)
        {
            string strPost = string.Format(@"FORM_ID=bc313251-2d8c-495f-ae19-4befcca7896e&COMPLETE_URL=http://travelogyclub.com/v2/formsubmitted.html&FIRST_NAME={0}&LAST_NAME={1}&EMAIL={2}&CUSTOMFIELD[TRIP_REQUEST]={3}&PHONE={4}",
                FIRST_NAME, LAST_NAME, EMAIL, TRIP_REQUEST, PHONE);

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create("https://service.capsulecrm.com/service/newlead");
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            String result = "";
            StreamWriter myWriter = null;

            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr =
               new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();

                // Close and clean up the StreamReader
                sr.Close();
            }

            return result;            
        }
    }
}
