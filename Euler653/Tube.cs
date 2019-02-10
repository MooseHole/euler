using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Euler653
{
    class Tube
    {
        private UInt64 _length;
        private List<Marble> _marbles;

        public Tube(UInt64 lengthMillimeters, int numMarbles)
        {
            _length = lengthMillimeters * Constants.DistanceMultiplier;
            SetupMarbles(numMarbles);
        }

        public List<Marble> Marbles
        {
            get => _marbles;
        }

        void SetupMarbles(int numMarbles)
        {
            Console.Write("v");
            RNG rng = new RNG();
            UInt64 previousEdgeMillimeters = 0;
            _marbles = new List<Marble>();
            for (int i = 0; i < numMarbles; i++)
            {
                UInt64 thisPositionMillimeters =
                    previousEdgeMillimeters + rng.GetInitialGap(i) + Constants.MarbleRadiusMillimeters;
                if (thisPositionMillimeters * Constants.DistanceMultiplier > _length)
                {
                    break;
                }

                bool thisMovingWest = rng.R(i) > 10_000_000;
                Marble thisMarble = new Marble(thisPositionMillimeters * Constants.DistanceMultiplier, thisMovingWest);
                _marbles.Add(thisMarble);
                previousEdgeMillimeters = thisPositionMillimeters + Constants.MarbleRadiusMillimeters;
                if (i > 0)
                {
                    thisMarble.PreviousMarble = _marbles[i - 1];
                    thisMarble.PreviousMarble.NextMarble = thisMarble;
                }
            }

            Console.WriteLine("^ Total Marbles:" + _marbles.Count);
        }

        void Step()
        {
            // First Marble
            UInt64 minDistance = _marbles[0].MovingWest ? _marbles[0].WestEdge : UInt64.MaxValue;

            // Last Marble
            int lastMarbleIndex = _marbles.Count - 1;
            for (int i = lastMarbleIndex; i >= 0; i--)
            {
                if (_marbles[i].IsLastMarble)
                {
                    UInt64 distance = _length + 1 - _marbles[i].Position;
                    minDistance = Math.Min(minDistance, distance);
                    lastMarbleIndex = i;
                    break;
                }
            }

            // For all middle marbles
            for (int i = 0; i < lastMarbleIndex; i++)
            {
                if (minDistance == 0)
                {
                    break;
                }

                UInt64 distance = _marbles[i].DistanceToCollision(_marbles[i].NextMarble);
                minDistance = Math.Min(minDistance, distance);
            }

            Parallel.ForEach(_marbles, thisMarble => ProcessMarble(thisMarble, minDistance));
            if (minDistance > 0)
            {
                // A collision is likely after the previous processing, so process collisions now.
                Parallel.ForEach(_marbles, thisMarble => ProcessMarble(thisMarble, 0));
            }
        }

        void ProcessMarble(Marble thisMarble, UInt64 stepDistance)
        {
            if (thisMarble.FellOut)
            {
                return;
            }

            if (thisMarble.Position > _length)
            {
                thisMarble.FellOut = true;
                return;
            }

            if (stepDistance == 0)
            {
                thisMarble.Collide();
            }
            else
            {
                thisMarble.Step(stepDistance);
            }
        }

        public int GetTotalDistanceOfMillimeters(int marbleNumberOneBased)
        {
            Marble thisMarble = _marbles[marbleNumberOneBased - 1];
            while (!thisMarble.FellOut)
            {
                CleanupFallenMarbles(marbleNumberOneBased);
                Step();
            }

            return (int)thisMarble.TravelDistanceMillimeters;
        }

        private void CleanupFallenMarbles(int dontCareIndex)
        {
            for (int i = _marbles.Count - 1; i >= 0; i--)
            {
                if (_marbles[i].FellOut)
                {
                    _marbles.RemoveAt(i);
                }
                else
                {
                    // They only fall out on the end
                    break;
                }
            }

            for (int i = _marbles.Count - 1; i >= 0; i--)
            {
                if (i > dontCareIndex && !_marbles[i].MovingWest)
                {
                    _marbles[i].FellOut = true;
                }
                else
                {
                    // They only fall out on the end
                    break;
                }
            }
        }
    }
}
