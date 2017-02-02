using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{
    internal class CapsupleCrmGateway
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FIRST_NAME"></param>
        /// <param name="LAST_NAME"></param>
        /// <param name="EMAIL"></param>
        /// <param name="TRIP_REQUEST"></param>
        /// <returns></returns>
        public async Task<string> CreateCapsuleLead(string FIRST_NAME, string LAST_NAME, string EMAIL, string TRIP_REQUEST)
        {
            string strPost = string.Format(@"FORM_ID=bc313251-2d8c-495f-ae19-4befcca7896e&COMPLETE_URL=http://travelogyclub.com/v2/formsubmitted.html&FIRST_NAME=TESTLEAD_{0}&LAST_NAME={1}&EMAIL=TESTLEAD_{2}&CUSTOMFIELD[TRIP_REQUEST]={3}",
                FIRST_NAME, LAST_NAME, EMAIL, TRIP_REQUEST);

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
