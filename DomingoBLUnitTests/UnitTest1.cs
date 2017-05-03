using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomingoBL;
using DomingoDAL;
using DomingoBL.Google;
using System.Collections.Generic;

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
            interests.Add(TripInterest.Beach);
            interests.Add(TripInterest.Mountain);
            var blError = TripManagerV2.GetSuggestedTrip("New Delhi", 14, interests, out suggestions);

            Assert.IsTrue(blError.ErrorCode == 0);
        }

        [TestMethod]
        public void Test_Google_Api_distancematrix()
        {
            var _api = new GoogleApi();
            _api.Test_distancematrix();
        }

        [TestMethod]
        public void Test_Google_Api_DistanceCalculation()
        {
            var _api = new GoogleApi();
            var d = _api._CalculateDistance("Chittorgarh", "Bangalore");
            Assert.IsTrue(d > 0);
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
