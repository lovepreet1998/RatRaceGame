using Microsoft.VisualStudio.TestTools.UnitTesting;
using RatRaceGame.LogicLayer;
using System;

namespace RatRaceGameTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFactory()
        {
            Punter punter = Factory.GetAPunter(3);
            bool answer = punter is Jack;
            Assert.AreEqual(answer, true);
        }
    }
}
