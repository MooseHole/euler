using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler653
{
    class Marble
    {
        private UInt64 _position;
        private double _travelDistance;
        private bool _movingWest;
        private bool _fellOut;

        public UInt64 Position
        {
            get => _position;
            private set => _position = value;
        }

        public UInt64 PositionMillimeters
        {
            get => _position / 10;
        }

        public UInt64 WestEdge
        {
            get => _position - Constants.MarbleRadius;
        }

        public UInt64 EastEdge
        {
            get => _position + Constants.MarbleRadius;
        }

        public bool MovingWest
        {
            get => _movingWest;
        }

        public double TravelDistance
        {
            get => _travelDistance;
            private set => _travelDistance = value;
        }

        public double TravelDistanceMillimeters
        {
            get => _travelDistance / 10;
        }

        public bool FellOut
        {
            get => _fellOut;
            set => _fellOut = value;
        }

        public Marble(UInt64 position, bool movingWest)
        {
            _position = position;
            _movingWest = movingWest;
            _travelDistance = 0;
            _fellOut = false;
        }

        public bool Collide()
        {
            _movingWest = !_movingWest;
            return _movingWest;
        }

        public bool SamePosition(Marble eastMarble)
        {
            return EastEdge == eastMarble.WestEdge;
        }

        public UInt64 DistanceToCollision(Marble eastMarble)
        {
            if (FellOut)
            {
                return UInt64.MaxValue;
            }

            if (MovingWest || !eastMarble.MovingWest)
            {
                return UInt64.MaxValue;
            }

            return (eastMarble.WestEdge - EastEdge) / 2;
        }

        public void Step(UInt64 distance)
        {
            _position += distance * (UInt64)(MovingWest ? -1 : 1);
            _travelDistance += distance;
        }
    }
}
