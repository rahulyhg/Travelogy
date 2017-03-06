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
            int timeDelay = 0;
            if(args.Count() > 0 && args[0] != null)
            {
                int.TryParse(args[0], out timeDelay);
            }

            if (timeDelay == 0) timeDelay = 30000;

            var url = "https://www.travelogyclub.com/circuit/HB9FF35F56_5324_4B94_8038_EA9C10C3EB76";

            int x = new Random().Next(4);
            Thread.Sleep(timeDelay);
            _Ping(url);          
        }

        private static void _Ping(string url)
        {
            try
            {
                using (StreamWriter file = new StreamWriter("C:\\_CODE\\travelogyportalheartbeattest.txt", true))
                {
                    string msg;
                    msg = string.Format("Heartbeat log ... \npinging URL: {0}", url);
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
