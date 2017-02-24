using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomingoBL;
using DomingoDAL;

namespace DomingoBLUnitTests
{
    [TestClass]
    public class UnitTest1
    {
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
