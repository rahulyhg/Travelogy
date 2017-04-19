using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL.Google
{
    public class GapiRootobject
    {
        public string[] destination_addresses { get; set; }
        public string[] origin_addresses { get; set; }
        public Row[] rows { get; set; }
        public string status { get; set; }
    }

    public class Row
    {
        public Element[] elements { get; set; }
    }

    public class Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }

    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class GoogleApi
    {
        private static string GoogleKey = "AIzaSyCSltrNf2Z914TjYdW-Q5ZknGPjB-BYP30";

        public void Test_distancematrix()
        {
            var result = MapsAPICall("https://maps.googleapis.com/maps/api/distancematrix/json?origins=kolkata&destinations=siliguri&mode=transit&language=en-GB&key=" + GoogleKey);
            if (!string.IsNullOrEmpty(result))
            {
                var t = JsonConvert.DeserializeObject<GapiRootobject>(result);
                if (t.destination_addresses != null)
                {
                    int x = 0;
                }
            }
        }

        public void Test_directions()
        {
            var result = MapsAPICall("https://maps.googleapis.com/maps/api/directions/json?origin=kolkata&destination=siliguri&mode=transit&language=en-GB&key=" + GoogleKey);
            if (!string.IsNullOrEmpty(result))
            {
                var t = JsonConvert.DeserializeObject<GapiRootobject>(result);
                if (t.destination_addresses != null)
                {
                    int x = 0;
                }
            }
        }

        private string MapsAPICall(string request)
        {
            //Pass request to google api with orgin and destination details
            HttpWebRequest _gRequest =
                (HttpWebRequest)WebRequest.Create(request);

            HttpWebResponse _gResponse = (HttpWebResponse)_gRequest.GetResponse();
            using (var streamReader = new StreamReader(_gResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;                
            }
        }
    }
}
