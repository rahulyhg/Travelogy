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
        public void Test_Create_Thread_Valid_Params()
        {
            var _tm = new ThreadMessage();
            var _blError = ThreadManager.CreateThread(_tm, "Test message");
            Assert.AreEqual(_blError.ErrorCode, 0);
        }
    }
}
