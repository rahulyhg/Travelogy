using DomingoDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomingoBL
{
    public enum TripInterest
    {
        Beach,        
	    Mountain,        
	    Trekking,        
	    Nightlife,        
	    Wildlife,       
	    Desert,      
	    Culture,        
	    Motorcycle,
	    History,     
	    Spiritual,
	    SocialWork,        
    }

    public class PlaceScore
    {
       public Place Place;
       public int Score;
    }

    public class TripManagerV2
    {
        //private static Connection

        public static DomingoBlError GetSuggestedTrip(string startingPoint, int Days, List<TripInterest> interests, out List<Place> suggestions)
        {
            suggestions = new List<Place>();
            Dictionary<int, PlaceScore> placeAndScore = new Dictionary<int, PlaceScore>();

            try
            {
                using (TravelogyDevEntities1 context = new TravelogyDevEntities1())
                {
                    // get the starting point
                    var startingPlace = context.Places.Where(p => p.Name == startingPoint).FirstOrDefault();
                    if (startingPlace == null)
                    {
                        throw new ApplicationException("Invalid Starting City");
                    }

                    // add starting point
                    suggestions.Add(startingPlace);

                    // calculate scores
                    CalculateScores(interests, placeAndScore, context);

                    // sort by scores
                    placeAndScore = placeAndScore.OrderByDescending(x => x.Value.Score).ToDictionary(x => x.Key, x => x.Value);

                    int tripDays = 0;
                    foreach(var x in placeAndScore)
                    {
                        suggestions.Add(x.Value.Place);
                        tripDays += x.Value.Place.UsualDays.Value;

                        if (tripDays > Days)
                            break;
                    }

                    suggestions = OptimizeTrip(suggestions, context);

                }

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
        /// <param name="suggestions"></param>
        /// <returns></returns>
        private static List<Place> OptimizeTrip(List<Place> suggestions, TravelogyDevEntities1 context)
        {
            // re-arrange by the distances
            Place next = null;
            Place current = suggestions.First();

            var OptimizedList = new List<Place>();
            OptimizedList.Add(suggestions.First());
            suggestions.RemoveAt(0);

            while (suggestions.Count > 0)
            {
                // now find the city closest to starting
                next = FindClosestPlace(current, suggestions, context);
                suggestions.Remove(next);
                OptimizedList.Add(next);
                current = next;
            }

            return OptimizedList;
        }

        private static Place FindClosestPlace(Place origin, List<Place> destinations, TravelogyDevEntities1 context)
        {
            if (destinations.Count == 1)
                return destinations.First();

            Place nearest = destinations.First();
            var _xConnection = context.Connections.Where(p => p.SourceId == origin.Id && p.DestinationId == nearest.Id).FirstOrDefault(); // City.Distance(origin, destinations.First());
            var distance = _xConnection.Distance.Value;

            foreach (var _place in destinations)
            {
                var tempDistance = context.Connections.Where(p => p.SourceId == origin.Id && p.DestinationId == _place.Id).FirstOrDefault().Distance.Value; 
                if (tempDistance < distance)
                {
                    nearest = _place;
                    distance = tempDistance;
                }
            }

            return nearest;
        }

        private static void CalculateScores(List<TripInterest> interests, Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            // loop through interests and calculate scores 
            foreach (var interest in interests)
            {
                switch (interest)
                {
                    case TripInterest.Beach:
                        CalculateBeachScores(placeAndScore, context);
                        break;

                    case TripInterest.Culture:
                        CalculateCultureScores(placeAndScore, context);
                        break;

                    case TripInterest.Desert:
                        CalculateDesertScores(placeAndScore, context);
                        break;

                    case TripInterest.History:
                        CalculateHistorycores(placeAndScore, context);
                        break;

                    case TripInterest.Motorcycle:
                        CalculateMotorcycleScores(placeAndScore, context);
                        break;

                    case TripInterest.Mountain:
                        CalculateMountainScores(placeAndScore, context);
                        break;

                    case TripInterest.Nightlife:
                        CalculateNightlifeScores(placeAndScore, context);
                        break;

                    case TripInterest.SocialWork:
                        CalculateSocialWorkScores(placeAndScore, context);
                        break;

                    case TripInterest.Spiritual:
                        CalculateSpiritualScores(placeAndScore, context);
                        break;

                    case TripInterest.Trekking:
                        CalculateTrekkingScores(placeAndScore, context);
                        break;

                    case TripInterest.Wildlife:
                        CalculateWildlifeScores(placeAndScore, context);
                        break;

                    default:
                        break;
                }

            }
        }

        private static void CalculateWildlifeScores(Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            var _places = context.Places.Where(p => p.Wildlife > 0);
            foreach (var _xPlace in _places)
            {
                if (placeAndScore.ContainsKey(_xPlace.Id))
                {
                    var placeInstance = placeAndScore[_xPlace.Id];
                    placeInstance.Score += _xPlace.Wildlife.Value;
                }
                else
                {
                    var placeInstance = new PlaceScore() { Place = _xPlace, Score = _xPlace.Wildlife.Value };
                    placeAndScore.Add(_xPlace.Id, placeInstance);
                }
            }
        }

        private static void CalculateTrekkingScores(Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            var _places = context.Places.Where(p => p.Trekking > 0);
            foreach (var _xPlace in _places)
            {
                if (placeAndScore.ContainsKey(_xPlace.Id))
                {
                    var placeInstance = placeAndScore[_xPlace.Id];
                    placeInstance.Score += _xPlace.Trekking.Value;
                }
                else
                {
                    var placeInstance = new PlaceScore() { Place = _xPlace, Score = _xPlace.Trekking.Value };
                    placeAndScore.Add(_xPlace.Id, placeInstance);
                }
            }
        }

        private static void CalculateSpiritualScores(Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            var _places = context.Places.Where(p => p.Spiritual > 0);
            foreach (var _xPlace in _places)
            {
                if (placeAndScore.ContainsKey(_xPlace.Id))
                {
                    var placeInstance = placeAndScore[_xPlace.Id];
                    placeInstance.Score += _xPlace.Spiritual.Value;
                }
                else
                {
                    var placeInstance = new PlaceScore() { Place = _xPlace, Score = _xPlace.Spiritual.Value };
                    placeAndScore.Add(_xPlace.Id, placeInstance);
                }
            }
        }

        private static void CalculateSocialWorkScores(Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            var _places = context.Places.Where(p => p.SocialWork > 0);
            foreach (var _xPlace in _places)
            {
                if (placeAndScore.ContainsKey(_xPlace.Id))
                {
                    var placeInstance = placeAndScore[_xPlace.Id];
                    placeInstance.Score += _xPlace.SocialWork.Value;
                }
                else
                {
                    var placeInstance = new PlaceScore() { Place = _xPlace, Score = _xPlace.SocialWork.Value };
                    placeAndScore.Add(_xPlace.Id, placeInstance);
                }
            }
        }

        private static void CalculateNightlifeScores(Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            var _places = context.Places.Where(p => p.Nightlife > 0);
            foreach (var _xPlace in _places)
            {
                if (placeAndScore.ContainsKey(_xPlace.Id))
                {
                    var placeInstance = placeAndScore[_xPlace.Id];
                    placeInstance.Score += _xPlace.Nightlife.Value;
                }
                else
                {
                    var placeInstance = new PlaceScore() { Place = _xPlace, Score = _xPlace.Nightlife.Value };
                    placeAndScore.Add(_xPlace.Id, placeInstance);
                }
            }
        }

        private static void CalculateMountainScores(Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            var _places = context.Places.Where(p => p.Mountain > 0);
            foreach (var _xPlace in _places)
            {
                if (placeAndScore.ContainsKey(_xPlace.Id))
                {
                    var placeInstance = placeAndScore[_xPlace.Id];
                    placeInstance.Score += _xPlace.Mountain.Value;
                }
                else
                {
                    var placeInstance = new PlaceScore() { Place = _xPlace, Score = _xPlace.Mountain.Value };
                    placeAndScore.Add(_xPlace.Id, placeInstance);
                }
            }
        }

        private static void CalculateMotorcycleScores(Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            var _places = context.Places.Where(p => p.Motorcycle > 0);
            foreach (var _xPlace in _places)
            {
                if (placeAndScore.ContainsKey(_xPlace.Id))
                {
                    var placeInstance = placeAndScore[_xPlace.Id];
                    placeInstance.Score += _xPlace.Motorcycle.Value;
                }
                else
                {
                    var placeInstance = new PlaceScore() { Place = _xPlace, Score = _xPlace.Motorcycle.Value };                    
                    placeAndScore.Add(_xPlace.Id, placeInstance);
                }
            }
        }

        private static void CalculateHistorycores(Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            var _places = context.Places.Where(p => p.History > 0);
            foreach (var _xPlace in _places)
            {
                if (placeAndScore.ContainsKey(_xPlace.Id))
                {
                    var placeInstance = placeAndScore[_xPlace.Id];
                    placeInstance.Score += _xPlace.History.Value;
                }
                else
                {
                    var placeInstance = new PlaceScore() { Place = _xPlace, Score = _xPlace.History.Value };                    
                    placeAndScore.Add(_xPlace.Id, placeInstance);
                }
            }
        }

        private static void CalculateDesertScores(Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            var _places = context.Places.Where(p => p.Desert > 0);
            foreach (var _xPlace in _places)
            {
                if (placeAndScore.ContainsKey(_xPlace.Id))
                {
                    var placeInstance = placeAndScore[_xPlace.Id];
                    placeInstance.Score += _xPlace.Desert.Value;
                }
                else
                {
                    var placeInstance = new PlaceScore() { Place = _xPlace, Score = _xPlace.Desert.Value };                    
                    placeAndScore.Add(_xPlace.Id, placeInstance);
                }
            }
        }

        private static void CalculateCultureScores(Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            var _places = context.Places.Where(p => p.Culture > 0);
            foreach (var _xPlace in _places)
            {
                if (placeAndScore.ContainsKey(_xPlace.Id))
                {
                    var placeInstance = placeAndScore[_xPlace.Id];
                    placeInstance.Score += _xPlace.Culture.Value;
                }
                else
                {
                    var placeInstance = new PlaceScore() { Place = _xPlace, Score = _xPlace.Culture.Value };                    
                    placeAndScore.Add(_xPlace.Id, placeInstance);
                }
            }
        }

        private static void CalculateBeachScores(Dictionary<int, PlaceScore> placeAndScore, TravelogyDevEntities1 context)
        {
            var beachPlaces = context.Places.Where(p => p.Beach > 0);
            foreach (var _xPlace in beachPlaces)
            {
                if (placeAndScore.ContainsKey(_xPlace.Id))
                {
                    var placeInstance = placeAndScore[_xPlace.Id];
                    placeInstance.Score += _xPlace.Beach.Value;
                }
                else
                {
                    var placeInstance = new PlaceScore() { Place = _xPlace, Score = _xPlace.Beach.Value };                    
                    placeAndScore.Add(_xPlace.Id, placeInstance);
                }
            }
        }
    }
}
