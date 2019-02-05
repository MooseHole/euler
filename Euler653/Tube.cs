using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler653
{
    class Tube
    {
        private UInt64 _length;
        private List<Marble> _marbles;

        public Tube(UInt64 lengthMillimeters, int numMarbles)
        {
            _length = lengthMillimeters * 10;
            SetupMarbles(numMarbles);
        }

        public List<Marble> Marbles
        {
            get => _marbles;
        }

        void SetupMarbles(int numMarbles)
        {
            RNG rng = new RNG();
            UInt64 previousEdgeMillimeters = 0;
            _marbles = new List<Marble>();
            for (int i = 0; i < numMarbles; i++)
            {
                UInt64 thisPositionMillimeters = previousEdgeMillimeters + rng.GetInitialGap(i) + Constants.MarbleRadiusMillimeters;
                bool thisMovingWest = rng.R(i) > 10_000_000;
                _marbles.Add(new Marble(thisPositionMillimeters * 10, thisMovingWest));
                previousEdgeMillimeters = thisPositionMillimeters + Constants.MarbleRadiusMillimeters;
            }
        }

        void Step()
        {
            UInt64 minDistance = _marbles[0].MovingWest ? _marbles[0].WestEdge : UInt64.MaxValue;

            for (int i = 0; i < _marbles.Count; i++)
            {
                Marble marble = _marbles[i];
                if (marble.FellOut)
                {
                    continue;
                }

                bool isLastMarble = i == _marbles.Count - 1;

                Marble nextMarble = isLastMarble ? marble : _marbles[i + 1];

                UInt64 distance = marble.DistanceToCollision(nextMarble);

                if (isLastMarble || nextMarble.FellOut)
                {
                    distance = _length + 1 - marble.Position;
                }

                minDistance = (UInt64) Math.Abs((int) Math.Min(minDistance, distance));
                if (minDistance == 0)
                {
                    break;
                }
            }

            for (int i = 0; i < _marbles.Count; i++)
            {
                Marble marble = _marbles[i];
                if (marble.FellOut)
                {
                    continue;
                }

                if (marble.Position > _length)
                {
                    marble.FellOut = true;
                    continue;
                }

                if (minDistance == 0)
                {
                    CollideWithPrevious(i);
                    CollideWithNext(i);
                }
                else
                {
                    marble.Step(minDistance);
                }
            }
        }

        void CollideWithPrevious(int marbleNumber)
        {
            if (marbleNumber == 0)
            {
                if (_marbles[marbleNumber].WestEdge <= 0)
                {
                    _marbles[marbleNumber].Collide();
                }
                return;
            }

            if (_marbles[marbleNumber - 1].SamePosition(_marbles[marbleNumber]))
            {
                _marbles[marbleNumber].Collide();
            }
        }


        void CollideWithNext(int marbleNumber)
        {
            if (marbleNumber == _marbles.Count - 1)
            {
                return;
            }

            if (_marbles[marbleNumber + 1].FellOut)
            {
                return;
            }

            if (_marbles[marbleNumber].SamePosition(_marbles[marbleNumber + 1]))
            {
                _marbles[marbleNumber].Collide();
            }
        }


        public int GetTotalDistanceOfMillimeters(int marbleNumberOneBased)
        {
            int marbleNumber = marbleNumberOneBased - 1;
            while (!_marbles[marbleNumber].FellOut)
            {
                Step();
            }

            return (int)_marbles[marbleNumber].TravelDistanceMillimeters;
        }
    }
}
