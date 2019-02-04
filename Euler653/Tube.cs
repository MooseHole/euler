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

        public Tube(UInt64 length, int numMarbles)
        {
            _length = length;
            SetupMarbles(numMarbles);
        }

        public List<Marble> Marbles
        {
            get => _marbles;
        }

        void SetupMarbles(int numMarbles)
        {
            RNG rng = new RNG();
            UInt64 previousEdge = 0;
            _marbles = new List<Marble>();
            for (int i = 0; i < numMarbles; i++)
            {
                UInt64 thisPosition = previousEdge + rng.GetInitialGap(i) + Constants.MarbleRadius;
                bool thisMovingWest = rng.R(i) > 10_000_000;
                _marbles.Add(new Marble(thisPosition, thisMovingWest));
                previousEdge = thisPosition + Constants.MarbleRadius;
            }
        }

        void Step()
        {
/*
            UInt64 minDistance = _marbles[0].WestEdge;

            for (int i = 1; i < _marbles.Count; i++)
            {
                Marble marble = _marbles[i];
                if (marble.FellOut)
                {
                    continue;
                }

                bool isLastMarble = i == _marbles.Count - 1;

                Marble nextMarble = isLastMarble ? marble : _marbles[i + 1];

                UInt64 distance = (nextMarble.WestEdge - marble.EastEdge) / 2;

                if (isLastMarble || nextMarble.FellOut)
                {
                    distance = _length - marble.EastEdge;
                }

                minDistance = Math.Min(minDistance, distance);
            }

            if (minDistance <= 0)
            {
                for (int i = 0; i < _marbles.Count; i++)
                {
                    Marble marble = _marbles[i];
                    if (marble.FellOut)
                    {
                        continue;
                    }

                    if (marble.Position >= _length)
                    {
                        marble.FellOut = true;
                        continue;
                    }

                    bool isLastMarble = i == _marbles.Count - 1;

                    if (marble.MovingWest && marble.WestEdge <= 0)
                    {
                        marble.Collide();
                    }
                    else if (!marble.MovingWest && !isLastMarble)
                    {
                        Marble nextMarble = _marbles[i + 1];
                        if (!nextMarble.FellOut && nextMarble.MovingWest &&
                            marble.EastEdge >= nextMarble.WestEdge)
                        {
                            marble.Collide();
                            nextMarble.Collide();
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < _marbles.Count; i++)
                {
                    _marbles[i].Step((int)minDistance);
                }
            }

*/

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

                CollideWithPrevious(i);
                CollideWithNext(i);

                marble.Step(1);
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
                _marbles[marbleNumber - 1].Collide();
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
                _marbles[marbleNumber + 1].Collide();
            }
        }


        public int GetTotalDistanceOf(int marbleNumberOneBased)
        {
            int marbleNumber = marbleNumberOneBased - 1;
            while (!_marbles[marbleNumber].FellOut)
            {
                Step();
            }

            return (int)_marbles[marbleNumber].TravelDistance;
        }
    }
}
