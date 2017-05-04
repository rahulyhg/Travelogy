using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{
    public class FlightStatsApi
    {
        private static string applicationId = "d151e0c9";
        private static string applicationKey = "40040aa8489cda5edd71c59ccfbec614";

        //Sample request GET "https://api.flightstats.com/flex/schedules/rest/v1/json/from/IXB/to/GAU/departing/2017/6/1?appId=d151e0c9&appKey=40040aa8489cda5edd71c59ccfbec614"

        // sample response
        /*
         * {
                 "request": {
                  "departureAirport": {
                   "requestedCode": "IXB",
                   "fsCode": "IXB"
                  },
                  "arrivalAirport": {
                   "requestedCode": "GAU",
                   "fsCode": "GAU"
                  },
                  "codeType": {},
                  "departing": true,
                  "date": {
                   "year": "2017",
                   "month": "6",
                   "day": "1",
                   "interpreted": "2017-06-01"
                  },
                  "url": "https://api.flightstats.com/flex/schedules/rest/v1/json/from/IXB/to/GAU/departing/2017/6/1"
                 },
                 "scheduledFlights": [
                  {
                   "carrierFsCode": "6E",
                   "flightNumber": "433",
                   "departureAirportFsCode": "IXB",
                   "arrivalAirportFsCode": "GAU",
                   "stops": 0,
                   "departureTime": "2017-06-01T16:00:00.000",
                   "arrivalTime": "2017-06-01T16:45:00.000",
                   "flightEquipmentIataCode": "320",
                   "isCodeshare": false,
                   "isWetlease": false,
                   "serviceType": "J",
                   "serviceClasses": [
                    "Y"
                   ],
                   "trafficRestrictions": [],
                   "codeshares": [],
                   "referenceCode": "114-1782181--"
                  },
                  {
                   "carrierFsCode": "G8",
                   "flightNumber": "153",
                   "departureAirportFsCode": "IXB",
                   "arrivalAirportFsCode": "GAU",
                   "stops": 0,
                   "departureTime": "2017-06-01T13:55:00.000",
                   "arrivalTime": "2017-06-01T14:50:00.000",
                   "flightEquipmentIataCode": "320",
                   "isCodeshare": false,
                   "isWetlease": false,
                   "serviceType": "J",
                   "serviceClasses": [
                    "Y"
                   ],
                   "trafficRestrictions": [],
                   "codeshares": [],
                   "referenceCode": "114-1782180--"
                  }
                 ],
                 "appendix": {
                  "airlines": [
                   {
                    "fs": "6E",
                    "iata": "6E",
                    "icao": "IGO",
                    "name": "IndiGo",
                    "active": true
                   },
                   {
                    "fs": "G8",
                    "iata": "G8",
                    "icao": "GOW",
                    "name": "GoAir",
                    "active": true
                   }
                  ],
                  "airports": [
                   {
                    "fs": "IXB",
                    "iata": "IXB",
                    "icao": "VEBD",
                    "name": "Bagdogra Airport",
                    "city": "Bagdogra",
                    "cityCode": "IXB",
                    "countryCode": "IN",
                    "countryName": "India",
                    "regionName": "Asia",
                    "timeZoneRegionName": "Asia/Kolkata",
                    "localTime": "2017-05-04T23:13:12.696",
                    "utcOffsetHours": 5.5,
                    "latitude": 26.68488,
                    "longitude": 88.324816,
                    "elevationFeet": 414,
                    "classification": 4,
                    "active": true
                   },
                   {
                    "fs": "GAU",
                    "iata": "GAU",
                    "icao": "VEGT",
                    "name": "Lokpriya Gopinath Bordoloi International Airport",
                    "city": "Guwahati",
                    "cityCode": "GAU",
                    "countryCode": "IN",
                    "countryName": "India",
                    "regionName": "Asia",
                    "timeZoneRegionName": "Asia/Kolkata",
                    "localTime": "2017-05-04T23:13:12.696",
                    "utcOffsetHours": 5.5,
                    "latitude": 26.105185,
                    "longitude": 91.588427,
                    "elevationFeet": 158,
                    "classification": 3,
                    "active": true
                   }
                  ],
                  "equipments": [
                   {
                    "iata": "320",
                    "name": "Airbus A320",
                    "turboProp": false,
                    "jet": true,
                    "widebody": false,
                    "regional": false
                   }
                  ]
                 }
                }
         */

        public class DepartureAirport
        {
            public string requestedCode { get; set; }
            public string fsCode { get; set; }
        }

        public class ArrivalAirport
        {
            public string requestedCode { get; set; }
            public string fsCode { get; set; }
        }

        public class CodeType
        {
        }

        public class Date
        {
            public string year { get; set; }
            public string month { get; set; }
            public string day { get; set; }
            public string interpreted { get; set; }
        }

        public class Request
        {
            public DepartureAirport departureAirport { get; set; }
            public ArrivalAirport arrivalAirport { get; set; }
            public CodeType codeType { get; set; }
            public bool departing { get; set; }
            public Date date { get; set; }
            public string url { get; set; }
        }

        public class ScheduledFlight
        {
            public string carrierFsCode { get; set; }
            public string flightNumber { get; set; }
            public string departureAirportFsCode { get; set; }
            public string arrivalAirportFsCode { get; set; }
            public int stops { get; set; }
            public string departureTime { get; set; }
            public string arrivalTime { get; set; }
            public string flightEquipmentIataCode { get; set; }
            public bool isCodeshare { get; set; }
            public bool isWetlease { get; set; }
            public string serviceType { get; set; }
            public List<string> serviceClasses { get; set; }
            public List<object> trafficRestrictions { get; set; }
            public List<object> codeshares { get; set; }
            public string referenceCode { get; set; }
        }

        public class Airline
        {
            public string fs { get; set; }
            public string iata { get; set; }
            public string icao { get; set; }
            public string name { get; set; }
            public bool active { get; set; }
        }

        public class Airport
        {
            public string fs { get; set; }
            public string iata { get; set; }
            public string icao { get; set; }
            public string name { get; set; }
            public string city { get; set; }
            public string cityCode { get; set; }
            public string countryCode { get; set; }
            public string countryName { get; set; }
            public string regionName { get; set; }
            public string timeZoneRegionName { get; set; }
            public string localTime { get; set; }
            public double utcOffsetHours { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public int elevationFeet { get; set; }
            public int classification { get; set; }
            public bool active { get; set; }
        }

        public class Equipment
        {
            public string iata { get; set; }
            public string name { get; set; }
            public bool turboProp { get; set; }
            public bool jet { get; set; }
            public bool widebody { get; set; }
            public bool regional { get; set; }
        }

        public class Appendix
        {
            public List<Airline> airlines { get; set; }
            public List<Airport> airports { get; set; }
            public List<Equipment> equipments { get; set; }
        }

        public class RootObject
        {
            public Request request { get; set; }
            public List<ScheduledFlight> scheduledFlights { get; set; }
            public Appendix appendix { get; set; }
        } 
    }
}
