using DomingoDAL;
using GoogleMapsApi;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
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
    

    public class GoogleApi
    {
        private static string GoogleKey = "AIzaSyCSltrNf2Z914TjYdW-Q5ZknGPjB-BYP30";      
        
        public void Test_distancematrix()
        {
            PopulateDistances();
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

        public double _CalculateDistance(string origin, string destination)
        {
            System.Threading.Thread.Sleep(2000);
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
    }
}


