using DomingoDAL;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
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
    
    public class LatLong
    {
        public double Latitude;
        public double Longitude;
    }

    public class GoogleApi
    {
        private static string GoogleKey = "AIzaSyCSltrNf2Z914TjYdW-Q5ZknGPjB-BYP30";

        public void PopulateTransitRoutes()
        {
            var originIds = new List<int>();
            var destinationIds = new List<int>();

            using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
            {
                var places = context.Places.Select(p => p);
                foreach(var p in places)
                {
                    originIds.Add(p.Id);
                    destinationIds.Add(p.Id);
                }
            }

            foreach(int xId in originIds)
            {
                foreach(int yId in destinationIds)
                {
                    if(xId != yId)
                    {
                        System.Threading.Thread.Sleep(200);
                        _SaveTransitDetails(xId, yId, new DateTime(2017, 6, 1, 2, 0, 0));

                        System.Threading.Thread.Sleep(200);
                        _SaveTransitDetails(xId, yId, new DateTime(2017, 6, 1, 8, 0, 0));

                        System.Threading.Thread.Sleep(200);
                        _SaveTransitDetails(xId, yId, new DateTime(2017, 6, 1, 14, 0, 0));
                    }                    
                }
            }            
        }

        public void PopulateAirportAddress()
        {
            double latitude, longitude;
            using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
            {
                var airports = context.Airports.Where(p => p.Id >= 100050);
                foreach(var airport in airports)
                {
                    if (airport.Lattitude.HasValue) continue;

                    _GeocodePlace(airport.Name, out latitude, out longitude);
                    if(latitude != 0.0 && longitude != 0.0)
                    {
                        airport.Lattitude = (decimal)latitude;
                        airport.Longitude = (decimal)longitude;
                        latitude = 0; longitude = 0;
                    }
                }

                context.SaveChanges();
            }                
        }

        public void PopulateAirportDistances()
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var placesWithAirports = context.Places.Where(p => p.NearestAirport.Length > 0);
                    foreach(var city in placesWithAirports)
                    {
                        var airport = context.Airports.Where(p => p.Code == city.NearestAirport).FirstOrDefault();
                        if (airport == null) continue;

                        if (city.NearestAirportDistance.HasValue) continue;

                        var origin = new LatLong { Latitude = (double)airport.Lattitude, Longitude = (double)airport.Longitude };
                        var destinaiton = new LatLong() { Latitude = (double)city.Lattitude, Longitude = (double)city.Longitude };

                        System.Threading.Thread.Sleep(1000);
                        var distance = _CalculateDistance(origin, destinaiton);
                        city.NearestAirportDistance = (decimal)distance;
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception)
            {
                
            }
        }

        public void PopulateAirportDriveTimes()
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var placesWithAirports = context.Places.Where(p => p.NearestAirport.Length > 0);
                    foreach (var city in placesWithAirports)
                    {
                        var airport = context.Airports.Where(p => p.Code == city.NearestAirport).FirstOrDefault();
                        if (airport == null) continue;

                        if (city.NearestAirportDriveTime.HasValue) continue;

                        var origin = new LatLong { Latitude = (double)airport.Lattitude, Longitude = (double)airport.Longitude };
                        var destinaiton = new LatLong() { Latitude = (double)city.Lattitude, Longitude = (double)city.Longitude };

                        System.Threading.Thread.Sleep(1000);
                        var driveTime = _CalculateDrivingTime(origin, destinaiton);
                        city.NearestAirportDriveTime = (decimal)driveTime;
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }

        public void PopulateAirportTransitTimes()
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var placesWithAirports = context.Places.Where(p => p.NearestAirport.Length > 0);
                    foreach (var city in placesWithAirports)
                    {
                        var airport = context.Airports.Where(p => p.Code == city.NearestAirport).FirstOrDefault();
                        if (airport == null) continue;

                        if (city.NearestAirportTransitTime.HasValue) continue;

                        var origin = new LatLong { Latitude = (double)airport.Lattitude, Longitude = (double)airport.Longitude };
                        var destinaiton = new LatLong() { Latitude = (double)city.Lattitude, Longitude = (double)city.Longitude };

                        System.Threading.Thread.Sleep(1000);
                        var _transitTime = _CalculateTransit(origin, destinaiton, new DateTime(2017, 6, 1, 13, 0, 0));
                        city.NearestAirportTransitTime = (decimal)_transitTime;
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }

        public void PopulateDistances()
        {
            using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
            {
                var sources = context.Places.ToList();
                var distances = context.Connections.Select(p => p);
                var destinations = sources;

                foreach(var source in sources)
                {
                    foreach(var destination in destinations)
                    {
                        if (source.Name == destination.Name)
                            continue;

                        var _dbDistance = distances.Where(x => x.SourceId == source.Id && x.DestinationId == destination.Id).FirstOrDefault() ;
                        if (_dbDistance != null)
                            continue;

                        double distanceKm = _CalculateDistance(source.Name, destination.Name);
                        if(distanceKm > 0)
                        {
                            context.Connections.Add(new Connection() { SourceId = source.Id, DestinationId = destination.Id, Distance = (decimal)distanceKm });
                            context.SaveChanges();
                        }
                    }                    
                    //System.Threading.Thread.Sleep(10000);
                }

                context.SaveChanges();
            }
        }

        public void PopulateDriveTimes()
        {
            using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
            {
                var sources = context.Places.ToList();
                var conns = context.Connections.Select(p => p);
                var destinations = sources;

                foreach (var source in sources)
                {
                    foreach (var destination in destinations)
                    {
                        if (source.Name == destination.Name)
                            continue;

                        var _conxn = conns.Where(x => x.SourceId == source.Id && x.DestinationId == destination.Id).FirstOrDefault();
                        if (_conxn != null && !_conxn.DrivingTime.HasValue)
                        {
                            double driveTime = _CalculateDrivingTime(source.Name, destination.Name);
                            if(driveTime > 0)
                            {
                                _conxn.DrivingTime = (decimal)driveTime;
                                context.SaveChanges();
                            }
                        }                        
                    }                    
                }

                context.SaveChanges();
            }
        }

        public void PopulateAirConnectivityTimes()
        {
            using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
            {
                var sources = context.Places.ToList();
                var conns = context.Connections.Select(p => p);
                var airportconns = context.AirportConnections.Select(p => p);
                var destinations = sources;

                foreach (var source in sources)
                {
                    foreach (var destination in destinations)
                    {
                        // if the source is destination - skip
                        if (source.Name == destination.Name)
                            continue;

                        // if the places do not have airports - skip
                        if (String.IsNullOrEmpty(source.NearestAirport) || String.IsNullOrEmpty(destination.NearestAirport))
                            continue;

                        // if the places are near the same airport - skip
                        if (source.NearestAirport == destination.NearestAirport)
                            continue;

                        // ok now that we have two airports - calculate flight time
                        int fromAirport = context.Airports.Where(p => p.Code == source.NearestAirport).FirstOrDefault().Id;
                        int toAirport = context.Airports.Where(p => p.Code == destination.NearestAirport).FirstOrDefault().Id;

                        var flights = airportconns.Where(p => p.SourceId == fromAirport && p.DestinationId == toAirport);
                        if (flights == null || flights.Count() == 0) // there are no connecting flightts
                            continue;

                        var averageFlightTime = flights.Average(p => p.FlightTime1);
                        var minFLightTime = flights.Min(p => p.FlightTime1);
                        var maxFlightTime = flights.Max(p => p.FlightTime1);

                        var conn = conns.Where(p => p.SourceId == source.Id && p.DestinationId == destination.Id).FirstOrDefault();
                        if(conn != null)
                        {
                            conn.MaxFlightTime = maxFlightTime + source.NearestAirportDriveTime + destination.NearestAirportDriveTime;
                            conn.MinFlightTime = minFLightTime + source.NearestAirportDriveTime + destination.NearestAirportDriveTime;
                            conn.AvgFlightTime = averageFlightTime + source.NearestAirportDriveTime + destination.NearestAirportDriveTime;
                            context.SaveChanges();
                        }
                       
                    }  
                }
            }
        }

        public void PopulateMinimumTransitTimes()
        {
            using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
            {
                var conns = context.Connections.Select(p => p);
                foreach(var conn in conns)
                {
                    if(conn.MinFlightTime != 0 && conn.MinFlightTime < conn.DrivingTime)
                    {
                        conn.MinimumTransitTime = (int)(conn.MinFlightTime * 60) ;
                    }
                    else
                    {
                        conn.MinimumTransitTime = (int)(conn.DrivingTime * 60);
                    }
                }

                context.SaveChanges();
            }
        }

        private void _GeocodePlace(string address, out double latitude, out double longitude)
        {
            latitude = 0; longitude = 0;
            try
            {
                var geocodeRequest = new GeocodingRequest
                {
                    Address = address,
                    ApiKey = GoogleKey
                };

                GeocodingResponse geocodeResponse = GoogleMaps.Geocode.Query(geocodeRequest);
                if (geocodeResponse.Status == Status.OK)
                {
                    latitude = geocodeResponse.Results.FirstOrDefault().Geometry.Location.Latitude;
                    longitude = geocodeResponse.Results.FirstOrDefault().Geometry.Location.Longitude;
                }
            }
            catch (Exception)
            {

            }
        }

        private void _SaveTransitDetails(int originId, int destinationId, DateTime depTime)
        {
            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    var origin = context.Places.Find(originId);
                    var destination = context.Places.Find(destinationId);

                    if (origin == null || destination == null) return;

                    var transitDirectionRequest = new DirectionsRequest
                    {
                        Origin = origin.Name,
                        Destination = destination.Name,
                        TravelMode = TravelMode.Transit,
                        DepartureTime = depTime,
                        ApiKey = GoogleKey
                    };

                    DirectionsResponse transitDirections = GoogleMaps.Directions.Query(transitDirectionRequest);

                    if (transitDirections.Status == DirectionsStatusCodes.OK)
                    {
                        string jsonStr = JsonConvert.SerializeObject(transitDirections);
                        string transitStart = transitDirections.Routes.FirstOrDefault().Legs.FirstOrDefault().DepartureTime.Text;
                        var _dbVal = context.Transits.Where(p => p.SourceId == originId
                                        && p.DestinationId == destinationId
                                        && p.departure_time == transitStart);

                        if(_dbVal.Count() == 0)
                        {
                            var _transitDetail = new Transit();

                            _transitDetail.SourceId = originId;
                            _transitDetail.DestinationId = destinationId;

                            _transitDetail.Distance = transitDirections.Routes.FirstOrDefault().Legs.FirstOrDefault().Distance.Value / 1000;
                            _transitDetail.departure_time = transitStart;
                            _transitDetail.arrival_time = transitDirections.Routes.FirstOrDefault().Legs.FirstOrDefault().ArrivalTime.Text;
                            _transitDetail.Transit_Time = (decimal)transitDirections.Routes.FirstOrDefault().Legs.FirstOrDefault().Duration.Value.TotalHours;

                            context.Transits.Add(_transitDetail);

                            context.SaveChanges();
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                string _debug = ex.Message;
            }
            
        }

        private double _CalculateTransit(LatLong origin, LatLong destination, DateTime depTime)
        {
            try
            {
                var transitDirectionRequest = new DirectionsRequest
                {
                    Origin = string.Format("{0},{1}", origin.Latitude, origin.Longitude),
                    Destination = string.Format("{0},{1}", destination.Latitude, destination.Longitude),
                    TravelMode = TravelMode.Transit,
                    DepartureTime = depTime,
                    ApiKey = GoogleKey
                };

                DirectionsResponse transitDirections = GoogleMaps.Directions.Query(transitDirectionRequest);
                if (transitDirections.Status == DirectionsStatusCodes.OK)
                {
                    string jsonStr = JsonConvert.SerializeObject(transitDirections);
                    return 0;
                }
            }
            catch (Exception)
            {

            }

            return 0;
        }

        private double _CalculateDrivingTime(string origin, string destination)
        {            
            try
            {
                var transitDirectionRequest = new DirectionsRequest
                {
                    Origin = origin,
                    Destination = destination,
                    TravelMode = TravelMode.Driving,
                    DepartureTime = DateTime.Now
                };

                DirectionsResponse transitDirections = GoogleMaps.Directions.Query(transitDirectionRequest);
                if (transitDirections.Status == DirectionsStatusCodes.OK)
                {
                    double totalTime = 0;
                    foreach (var route in transitDirections.Routes)
                    {
                        foreach (var routeLeg in route.Legs)
                        {
                            totalTime += routeLeg.Duration.Value.TotalSeconds;
                        }
                    }

                    return totalTime / 3600.0;
                }
            }
            catch (Exception)
            {                
            }

            return 0;
        }

        private double _CalculateDrivingTime(LatLong origin, LatLong destination)
        {
            try
            {
                var transitDirectionRequest = new DirectionsRequest
                {
                    Origin = string.Format("{0},{1}", origin.Latitude, origin.Longitude),
                    Destination = string.Format("{0},{1}", destination.Latitude, destination.Longitude),
                    TravelMode = TravelMode.Driving,
                    DepartureTime = DateTime.Now
                };

                DirectionsResponse transitDirections = GoogleMaps.Directions.Query(transitDirectionRequest);
                if (transitDirections.Status == DirectionsStatusCodes.OK)
                {
                    double totalTime = 0;
                    foreach (var route in transitDirections.Routes)
                    {
                        foreach (var routeLeg in route.Legs)
                        {
                            totalTime += routeLeg.Duration.Value.TotalSeconds;
                        }
                    }

                    return totalTime / 3600.0;
                }
            }
            catch (Exception)
            {
            }

            return 0;
        }

        private double _CalculateDistance(string origin, string destination)
        {            
            try
            {
                var transitDirectionRequest = new DirectionsRequest
                {
                    Origin = origin,
                    Destination = destination,
                    TravelMode = TravelMode.Driving,
                    DepartureTime = DateTime.Now
                };

                DirectionsResponse transitDirections = GoogleMaps.Directions.Query(transitDirectionRequest);
                if (transitDirections.Status == DirectionsStatusCodes.OK)
                {
                    int distanceMeters = 0;
                    foreach (var route in transitDirections.Routes)
                    {
                        foreach (var routeLeg in route.Legs)
                        {
                            distanceMeters += routeLeg.Distance.Value;
                        }
                    }

                    return distanceMeters/1000.00;
                }
            }
            catch (Exception ex)
            {
            }

            return 0;
        }

        private double _CalculateDistance(LatLong origin, LatLong destination)
        {
            try
            {
                var transitDirectionRequest = new DirectionsRequest
                {
                    Origin = string.Format("{0},{1}", origin.Latitude, origin.Longitude),
                    Destination = string.Format("{0},{1}", destination.Latitude, destination.Longitude),
                    TravelMode = TravelMode.Driving,
                    DepartureTime = DateTime.Now
                };

                DirectionsResponse transitDirections = GoogleMaps.Directions.Query(transitDirectionRequest);
                if (transitDirections.Status == DirectionsStatusCodes.OK)
                {
                    int distanceMeters = 0;
                    foreach (var route in transitDirections.Routes)
                    {
                        foreach (var routeLeg in route.Legs)
                        {
                            distanceMeters += routeLeg.Distance.Value;
                        }
                    }

                    return distanceMeters / 1000.00;
                }
            }
            catch (Exception ex)
            {
            }

            return 0;
        }
    }
}


