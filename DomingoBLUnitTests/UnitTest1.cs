using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomingoBL;
using DomingoDAL;
using DomingoBL.Google;
using System.Collections.Generic;
using DomingoBL.FlightStatsApi;
using System.Text;

namespace DomingoBLUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSuggestions()
        {
            List<Place> suggestions = null;
            List<TripInterest> interests = new List<TripInterest>();
            interests.Add(TripInterest.Mountain);
            interests.Add(TripInterest.History);
            var blError = TripManagerV2.GetSuggestedTrip("New Delhi", 20, interests, out suggestions);

            var sb = new StringBuilder();
            foreach(var suggestion in suggestions)
            {
                sb.AppendFormat("--{0}--", suggestion.Name);
            }

            var sug = sb.ToString();
            Assert.IsTrue(blError.ErrorCode == 0);
        }

        [TestMethod]
        public void Test_Google_Api_PopulateDistances()
        {
            var _api = new GoogleApi();
            _api.PopulateDistances();
        }

        [TestMethod]
        public void Test_Google_Api_PopulateDriveTimes()
        {
            var _api = new GoogleApi();
            _api.PopulateDriveTimes();
        }

        [TestMethod]
        public void Test_Google_Api_PopulateAirConnectivityTimes()
        {
            var _api = new GoogleApi();
            _api.PopulateAirConnectivityTimes();
        }

        [TestMethod]
        public void Test_Google_Api_PopulateMinimumTransitTimes()
        {
            var _api = new GoogleApi();
            _api.PopulateMinimumTransitTimes();
        }

        [TestMethod]
        public void Test_Google_Api_PopulateAirportDriveTimes()
        {
            var _api = new GoogleApi();
            _api.PopulateAirportDriveTimes();
        }

        [TestMethod]
        public void Test_Google_Api_PopulateAirportAddress()
        {
            var _api = new GoogleApi();
            _api.PopulateAirportAddress();
        }

        [TestMethod]
        public void Test_Google_Api_PopulateAirportTransitTimes()
        {
            var _api = new GoogleApi();
            _api.PopulateAirportTransitTimes();
        }

        [TestMethod]
        public void Test_Google_Api_PopulateAirportDistances()
        {
            var _api = new GoogleApi();
            _api.PopulateAirportDistances();
        }

        [TestMethod]
        public void Test_Google_Api_DistanceCalculation()
        {
            //var _api = new GoogleApi();
            //var d = _api._CalculateDrivingTime("Chittorgarh", "Bangalore");            
            //Assert.IsTrue(d > 0);
        }

        [TestMethod]
        public void Test_Google_Api_PopulateTransitRoutes()
        {
            var _api = new GoogleApi();
            _api.PopulateTransitRoutes();
        }

        [TestMethod]
        public void Test_FlightStatsApi_PopulateAllFlights()
        {
            FlightStatsApiGateway.PopulateAllFlights();
        }

        [TestMethod]
        public void Test_FlightStatsApi_FindAndSaveFlights()
        {
            FlightStatsApiGateway.FindAndSaveFlights(100033, 100075, 2017, 6, 15);
        }

        

        [TestMethod]
        public void Test_Create_CRM_LEAD_Valid_Params()
        {
            var _gateWay = new CapsupleCrmGateway();
            var x = DateTime.Now.Millisecond.ToString();
            _gateWay.CreateCapsuleParty("FNAME" + x, "LNAME" + x, "EMAIL" + x + "@abc.com", "PHONE" + x, "test trip request");
        }

        [TestMethod]
        public void Test_Get_Currency_Xchange_rate()
        {
            double _xcg = 0D;   
            var _blError = CurrencyConvertGateway.GetCurrencyExchangeRate("INR", "GBP", out _xcg);
            Assert.IsTrue(_xcg >= 0D);
        }
    }
}
