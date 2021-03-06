﻿using System;

namespace Euler653
{
    class Marble
    {
        private UInt64 _position;
        private UInt64 _travelDistance;
        private bool _movingWest;
        private bool _fellOut;
        private Marble _previousMarble;
        private Marble _nextMarble;
        private bool _recordDistance;

        public UInt64 Position
        {
            get => _position;
            set => _position = value;
        }

        public Marble PreviousMarble
        {
            get => _previousMarble;
            set => _previousMarble = value;
        }

        public Marble NextMarble
        {
            get => _nextMarble;
            set => _nextMarble = value;
        }

        public UInt64 PositionMillimeters
        {
            get => _position / Constants.DistanceMultiplier;
        }

        public UInt64 WestEdge
        {
            get => _position - Constants.MarbleRadius;
        }

        public UInt64 EastEdge
        {
            get => _position + Constants.MarbleRadius;
        }

        public bool MovingEast => !_movingWest;

        public bool MovingWest
        {
            get => _movingWest;
            private set => _movingWest = value;
        }

        public UInt64 TravelDistance
        {
            get => _travelDistance;
        }

        public double TravelDistanceMillimeters
        {
            get => (double)_travelDistance / Constants.DistanceMultiplier;
            set => _travelDistance = (UInt64)(value * Constants.DistanceMultiplier);
        }

        public bool FellOut
        {
            get => _fellOut;
            set
            {
                if (_fellOut != value)
                {
                    _fellOut = value;
                    if (value)
                    {
                        Console.Write(".");
                    }
                }
            }
        }

        public bool IsLastMarble
        {
            get => !FellOut && (_nextMarble == null || _nextMarble.FellOut);
        }

        public bool IsWestmostMarble
        {
            get => _previousMarble == null;
        }

        public bool TouchingPrevious
        {
            get
            {
                if (PreviousMarble == null)
                {
                    return false;
                }

                return PreviousMarble.SamePosition(this);
            }
        }

        public bool TouchingNext
        {
            get
            {
                if (FellOut || IsLastMarble)
                {
                    return false;
                }

                return SamePosition(NextMarble);
            }
        }

        public Marble(UInt64 position, bool movingWest, bool recordDistance = false)
        {
            _position = position;
            _movingWest = movingWest;
            _travelDistance = 0;
            _fellOut = false;
            _previousMarble = null;
            _nextMarble = null;
            _recordDistance = recordDistance;
        }

        public bool RecordDistance
        {
            set { _recordDistance = value; }
        }

        public void Collide()
        {
            if (TouchingPrevious)
            {
                _movingWest = !_movingWest;
                PreviousMarble.MovingWest = !PreviousMarble.MovingWest;
            }
            else if (TouchingNext)
            {
                _movingWest = !_movingWest;
                NextMarble.MovingWest = !NextMarble.MovingWest;
            }
            else if (IsWestmostMarble && MovingWest && WestEdge == 0)
            {
                _movingWest = !_movingWest;
            }
        }

        public bool SamePosition(Marble eastMarble)
        {
            if (eastMarble == null)
                return false;

            return !MovingWest && eastMarble.MovingWest && EastEdge == eastMarble.WestEdge;
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

            return eastMarble.WestEdge - EastEdge;
        }

        public void Step(UInt64 distance)
        {
            _position += distance * (UInt64)(MovingWest ? -1 : 1);
//            if (_recordDistance)
            {
                _travelDistance += distance;
            }
        }
    }
}
