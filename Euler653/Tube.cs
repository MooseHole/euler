using System;
using System.Collections.Generic;
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
            Console.Write("v");
            RNG rng = new RNG();
            UInt64 previousEdgeMillimeters = 0;
            _marbles = new List<Marble>();
            for (int i = 0; i < numMarbles; i++)
            {
                UInt64 thisPositionMillimeters =
                    previousEdgeMillimeters + rng.GetInitialGap(i) + Constants.MarbleRadiusMillimeters;
                if (thisPositionMillimeters * 10 > _length)
                {
                    break;
                }

                bool thisMovingWest = rng.R(i) > 10_000_000;
                Marble thisMarble = new Marble(thisPositionMillimeters * 10, thisMovingWest);
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
            UInt64 minDistance = _marbles[0].MovingWest ? _marbles[0].WestEdge : UInt64.MaxValue;

            foreach(Marble thisMarble in _marbles)
            {
                if (minDistance == 0)
                {
                    break;
                }

                if (thisMarble.FellOut)
                {
                    continue;
                }

                UInt64 distance = UInt64.MaxValue;

                if (thisMarble.IsLastMarble)
                {
                    distance = _length + 1 - thisMarble.Position;
                }
                else
                {
                    distance = thisMarble.DistanceToCollision(thisMarble.NextMarble);
                }

                minDistance = Math.Min(minDistance, distance);
            }

            Parallel.ForEach(_marbles, thisMarble => ProcessMarble(thisMarble, minDistance));
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
                CleanupFallenMarbles();
                Step();
            }

            return (int)thisMarble.TravelDistanceMillimeters;
        }

        private void CleanupFallenMarbles()
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
                    return;
                }
            }
        }
    }
}
