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
            Tube tube = new Tube(expectedLength, expectedMarbles, expectedCheck);
            Assert.AreEqual(expectedMarbles, tube.Marbles.Count);

            tube = new Tube(expectedLength, expectedMarbles, expectedCheck);
            tube = new Tube("marbles.txt");
            Assert.AreEqual(expectedMarbles, tube.Marbles.Count);
        }

        [TestMethod]
        public void InitialPositionsTest()
        {
            Tube tube = new Tube(expectedLength, expectedMarbles, expectedCheck);
            Assert.AreEqual((UInt64)127, tube.Marbles[0].PositionMillimeters);
            Assert.AreEqual((UInt64)579, tube.Marbles[1].PositionMillimeters);
            Assert.AreEqual((UInt64)772, tube.Marbles[2].PositionMillimeters);

            tube = new Tube(expectedLength, expectedMarbles, expectedCheck);
            tube = new Tube("marbles.txt");
            Assert.AreEqual((UInt64)127, tube.Marbles[0].PositionMillimeters);
            Assert.AreEqual((UInt64)579, tube.Marbles[1].PositionMillimeters);
            Assert.AreEqual((UInt64)772, tube.Marbles[2].PositionMillimeters);
        }

        [TestMethod]
        public void InitialDirectionsTest()
        {
            Tube tube = new Tube(expectedLength, expectedMarbles, expectedCheck);
            Assert.AreEqual(false, tube.Marbles[0].MovingWest);
            Assert.AreEqual(true, tube.Marbles[1].MovingWest);
            Assert.AreEqual(true, tube.Marbles[2].MovingWest);

            tube = new Tube(expectedLength, expectedMarbles, expectedCheck);
            tube = new Tube("marbles.txt");
            Assert.AreEqual(false, tube.Marbles[0].MovingWest);
            Assert.AreEqual(true, tube.Marbles[1].MovingWest);
            Assert.AreEqual(true, tube.Marbles[2].MovingWest);
        }

        [TestMethod]
        public void OutputExpectedTest()
        {
            Tube tube = new Tube(expectedLength, expectedMarbles, expectedCheck);
            Assert.AreEqual(expectedTotalDistance, tube.GetTotalDistanceOfMillimeters());

            tube = new Tube(expectedLength, expectedMarbles, expectedCheck);
            tube = new Tube("marbles.txt");
            Assert.AreEqual(expectedTotalDistance, tube.GetTotalDistanceOfMillimeters());

            tube = new Tube("marbles.txt");
            Assert.AreEqual(expectedTotalDistance, tube.GetTotalDistanceOfMillimeters());
        }

        [TestMethod]
        public void Output2Test()
        {
            Tube tube = new Tube(10000, 11, 6);
            Assert.AreEqual(11780, tube.GetTotalDistanceOfMillimeters());

            tube = new Tube(10000, 11, 6);
            tube = new Tube("marbles.txt");
            Assert.AreEqual(11780, tube.GetTotalDistanceOfMillimeters());
        }

        [TestMethod]
        public void Output3Test()
        {
            Tube tube = new Tube(100000, 101, 51);
            Assert.AreEqual(114101, tube.GetTotalDistanceOfMillimeters());

            tube = new Tube(100000, 101, 51);
            tube = new Tube("marbles.txt");
            Assert.AreEqual(114101, tube.GetTotalDistanceOfMillimeters());
        }
    }
}
