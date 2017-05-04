using DomingoDAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL.FlightStatsApi
{
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

    public class FlightStatsApiGateway
    {
        private static string applicationId = "d151e0c9";
        private static string applicationKey = "40040aa8489cda5edd71c59ccfbec614";

        public static void PopulateAllFlights()
        {
            var fromIDs = new List<int>();
            var toIDs = new List<int>();

            using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
            {
                var airports = context.Airports.Where(p => p.Class == "A" || p.Class == "B");
                foreach(var airport in airports)
                {
                    fromIDs.Add(airport.Id);
                    toIDs.Add(airport.Id);
                }
            }

            foreach(var fromId in fromIDs)
            {
                foreach(var toId in toIDs)
                {
                    if (fromId == toId)
                        continue;
                    
                    //System.Threading.Thread.Sleep(1000);
                    FindAndSaveFlights(fromId, toId, 2017, 6, 2);                    
                }
            }
        }
                
        public static void FindAndSaveFlights(int fromId, int toId, int year, int month, int day)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var fromAirport = context.Airports.Find(fromId);
                    var toAirport = context.Airports.Find(toId);

                    if (fromAirport == null || toAirport == null)
                    {
                        throw new ApplicationException("Invalid Airport ID");
                    }

                    var _dbVal = context.AirportConnections.Where(p =>
                                          p.SourceId == fromId && p.DestinationId == toId).FirstOrDefault();

                    if (_dbVal != null)
                        return;

                    string payload = string.Format("https://api.flightstats.com/flex/schedules/rest/v1/json/from/{0}/to/{1}/departing/{2}/{3}/{4}?appId={5}&appKey={6}&extendedOptions=includeDirects",
                        fromAirport.Code, toAirport.Code, year, month, day, applicationId, applicationKey);

                    var response = _SendRequest(payload);

                    foreach (var schedule in response.scheduledFlights)
                    {
                        DateTime depTime, arrTime;

                        if(!DateTime.TryParse(schedule.departureTime, out depTime)
                            || (!DateTime.TryParse(schedule.arrivalTime, out arrTime)))
                        {
                            throw new ApplicationException("Invalid Flight time");
                        }

                        string flightNumber = string.Format("{0}-{1}", schedule.carrierFsCode, schedule.flightNumber);

                        _dbVal = context.AirportConnections.Where(p =>
                                          p.SourceId == fromId && p.DestinationId == toId
                                          && p.FlightNumber == flightNumber).FirstOrDefault();

                        // duplicate flight
                        if (_dbVal != null)
                            return;

                        var flight = new AirportConnection();
                        flight.SourceId = fromId;
                        flight.DestinationId = toId;
                        flight.Stops = schedule.stops;
                        flight.FlightNumber = flightNumber;
                        flight.DepartureTime = depTime.TimeOfDay.ToString();
                        flight.ArrivalTime = arrTime.TimeOfDay.ToString();
                        var flightDuration = arrTime - depTime;
                        flight.FlightTime1 = (decimal)flightDuration.TotalMinutes / (decimal)60.0;

                        context.AirportConnections.Add(flight);
                        context.SaveChanges();                        
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }


        private static RootObject _SendRequest(string payload)
        {
            var responseObject = new RootObject();            

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(payload);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                responseObject = JsonConvert.DeserializeObject<RootObject>(responseString);

            }
            catch (Exception ex)
            {
                
            }

            return responseObject;
        }
    }
}

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
