using Microsoft.VisualStudio.TestTools.UnitTesting;
using OddMonitor.Models;
using OddMonitor.VPGame.Json;

namespace OddMonitor.Test.VPGame.Json
{
    [TestClass]
    public class VpEnumTest
    {
        [TestMethod]
        public void TestWagerStatus()
        {
            var e = VpEnum.GetWagerStatus("Right Now");

            Assert.AreEqual(WagerStatus.Live, e);
        }
    }
}