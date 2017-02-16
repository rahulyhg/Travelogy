using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Heartbeat
{
    class Program
    {
        static void Main(string[] args)
        {
            var urlList = new List<string>()
            {
                "http://portal-demo.travelogyclub.com/India",
                "http://portal-demo.travelogyclub.com/",
                "http://portal-demo.travelogyclub.com/Circuit/Tanzania",
                "http://portal-demo.travelogyclub.com/Circuit/Russia"
            };

            int x = new Random().Next(4);

            _Ping(urlList[x]);          
        }

        private static void _Ping(string url)
        {
            try
            {
                using (StreamWriter file = new StreamWriter("C:\\_CODE\\travelogyportalheartbeattest.txt", true))
                {
                    string msg;
                    msg = string.Format("Heartbeat log ... pinging URL: {0}", url);
                    Console.WriteLine(msg);
                    file.WriteLine(msg);

                    WebRequest myReq = WebRequest.Create(url);
                    myReq.Timeout = 5000;
                    WebResponse response = myReq.GetResponse();
                    msg = string.Format("Heartbeat log TIME: {0} RESPONSE: {1}", DateTime.Now, ((HttpWebResponse)response).StatusDescription);
                    Console.WriteLine(msg);
                    file.WriteLine(msg);
                    file.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Heartbeat log ERROR: Time: {0} -- Error Message: {1}", DateTime.Now, ex.Message));
            }
        }
    }
}
