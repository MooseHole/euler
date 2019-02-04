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
        private bool _andAHalf;

        public UInt64 Position
        {
            get => _position;
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
            return EastEdge == eastMarble.WestEdge && _andAHalf == eastMarble._andAHalf;
        }

        public void Step(int amount)
        {
            if (MovingWest)
            {
                if (_andAHalf)
                {
                    _andAHalf = false;
                }
                else
                {
                    _position -= 1;
                    _andAHalf = true;
                }
            }
            else
            {
                if (_andAHalf)
                {
                    _position += 1;
                    _andAHalf = false;
                }
                else
                {
                    _andAHalf = true;
                }
            }

            _travelDistance += 0.5;
        }
    }
}
