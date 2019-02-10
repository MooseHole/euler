using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Euler653
{
    [TestClass]
    public class TubeTest
    {
        private const int expectedLength = 5000;
        private const int expectedMarbles = 3;
        private const int expectedCheck = 2;
        private const int expectedTotalDistance = 5519;

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestCleanup]
        public void CleanUp()
        {
        }

        [TestMethod]
        public void MarbleFillTest()
        {
            Tube tube = new Tube(expectedLength, expectedMarbles);

            Assert.AreEqual(expectedMarbles, tube.Marbles.Count);
        }

        [TestMethod]
        public void InitialPositionsTest()
        {
            Tube tube = new Tube(expectedLength, expectedMarbles);

            Assert.AreEqual((UInt64)127, tube.Marbles[0].PositionMillimeters);
            Assert.AreEqual((UInt64)579, tube.Marbles[1].PositionMillimeters);
            Assert.AreEqual((UInt64)772, tube.Marbles[2].PositionMillimeters);
        }

        [TestMethod]
        public void InitialDirectionsTest()
        {
            Tube tube = new Tube(expectedLength, expectedMarbles);

            Assert.AreEqual(false, tube.Marbles[0].MovingWest);
            Assert.AreEqual(true, tube.Marbles[1].MovingWest);
            Assert.AreEqual(true, tube.Marbles[2].MovingWest);
        }

        [TestMethod]
        public void OutputExpectedTest()
        {
            Tube tube = new Tube(expectedLength, expectedMarbles);
            Assert.AreEqual(expectedTotalDistance, tube.GetTotalDistanceOfMillimeters(expectedCheck));
        }

        [TestMethod]
        public void Output2Test()
        {
            Tube tube = new Tube(10000, 11);
            Assert.AreEqual(11780, tube.GetTotalDistanceOfMillimeters(6));
        }

        [TestMethod]
        public void Output3Test()
        {
            Tube tube = new Tube(100000, 101);
            Assert.AreEqual(114101, tube.GetTotalDistanceOfMillimeters(51));
        }

        [Ignore]
        [TestMethod]
        public void OutputSolutionTest()
        {
            Tube tube = new Tube(1000000000, 1000001);
            Assert.AreEqual(114101, tube.GetTotalDistanceOfMillimeters(500001));
        }
    }
}
