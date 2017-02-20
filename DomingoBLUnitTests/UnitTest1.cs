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
            _gateWay.CreateCapsuleParty("FNAME" + x, "LNAME" + x, "EMAIL" + x + "@abc.com" , "PHONE" + x , "test trip request");
        }
    }
}
