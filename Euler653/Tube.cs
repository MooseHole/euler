using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Euler653
{
    class Tube
    {
        private UInt64 _length;
        private List<Marble> _marbles;
        private int _checkMarble;
        private int _currentFellout = 0;

        public Tube(UInt64 lengthMillimeters, int numMarbles, int checkNum)
        {
            _length = lengthMillimeters * Constants.DistanceMultiplier;
            _checkMarble = checkNum - 1;
            SetupMarbles(numMarbles);
        }

        public void Swap(int index1, int index2)
        {
            Marble temp = _marbles[index1];
            _marbles[index1] = _marbles[index2];
            _marbles[index2] = temp;
        }

        public void SortByPosition()
        {
            Marble thisMarble = null;
            // Find west most marble
            for (int i = 0; i < _marbles.Count; ++i)
            {
                if (_marbles[i].PreviousMarble == null)
                {
                    thisMarble = _marbles[i];
                    break;
                }
            }

            _marbles.Clear();
            while (thisMarble != null)
            {
                _marbles.Add(thisMarble);
                thisMarble = thisMarble.NextMarble;
            }
        }

        public void SortByDistanceTraveled()
        {
            // Sort by amount of distance traveled.
            // Whenever a marble's distance traveled increases, bubble sort it into the list.
        }

        public Tube(string filename)
        {
            Filename = filename;
            if (File.Exists(Filename))
            {
                _marbles = new List<Marble>();
                StreamReader reader = File.OpenText(Filename);
                string line;
                int marbleIndex = 0;
                bool checkNum = false;
                bool setCheckMarble = false;
                while ((line = reader.ReadLine()) != null)
                {
                    string delimiters = "" + Constants.WestDelimiter + Constants.EastDelimiter +
                                        Constants.CheckDelimiter + Constants.CheckDistanceDelimiter;
                    string[] parts = Regex.Split(line, @"([" + delimiters + @"])");
                    double p;
                    bool firstNumber = true;
                    Marble thisMarble = null;
                    foreach (string part in parts)
                    {
                        if (double.TryParse(part, out p))
                        {
                            if (firstNumber)
                            {
                                _length = (UInt64)(p * Constants.DistanceMultiplier);
                                firstNumber = false;
                            }
                            else if (checkNum)
                            {
                                _checkMarble = (int)p - 1;
                                checkNum = false;
                            }
                            else if (setCheckMarble)
                            {
                                _marbles[_checkMarble].TravelDistanceMillimeters = p;
                                _marbles[_checkMarble].RecordDistance = true;
                                setCheckMarble = false;
                            }
                            else
                            {
                                thisMarble.Position = (UInt64)(p * Constants.DistanceMultiplier);
                                _marbles.Add(thisMarble);
                                if (marbleIndex > 0)
                                {
                                    thisMarble.PreviousMarble = _marbles[marbleIndex - 1];
                                    thisMarble.PreviousMarble.NextMarble = thisMarble;
                                }
                                marbleIndex++;
                            }
                        }
                        else
                        {
                            switch (part[0])
                            {
                                case Constants.CheckDelimiter:
                                    checkNum = true;
                                    break;
                                case Constants.CheckDistanceDelimiter:
                                    setCheckMarble = true;
                                    break;
                                case Constants.WestDelimiter:
                                    thisMarble = new Marble(0, true);
                                    break;
                                case Constants.EastDelimiter:
                                    thisMarble = new Marble(0, false);
                                    break;
                            }
                        }
                    }
                }

                reader.Close();
            }
            else
            {
                Console.Write("ERROR: Filename " + Filename + " does not exist!");
            }

            Console.WriteLine("^ Total Marbles:" + _marbles.Count + " " + DateTime.Now);
        }

        public List<Marble> Marbles
        {
            get => _marbles;
        }

        public string Filename { get; private set; }

        void WriteMarbles()
        {
            using (StreamWriter outputFile = new StreamWriter(Filename))
            {
                outputFile.Write(_length / Constants.DistanceMultiplier);
                foreach (Marble marble in _marbles)
                {
                    outputFile.Write(marble.MovingWest ? Constants.WestDelimiter : Constants.EastDelimiter);
                    outputFile.Write(marble.PositionMillimeters);
                }
                outputFile.Write(Constants.CheckDelimiter);
                outputFile.Write(_checkMarble + 1);
                outputFile.Write(Constants.CheckDistanceDelimiter);
                outputFile.Write(_marbles[_checkMarble].TravelDistanceMillimeters);

                outputFile.Close();
            }
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
                Marble thisMarble = new Marble(thisPositionMillimeters * Constants.DistanceMultiplier, thisMovingWest, i == _checkMarble);
                _marbles.Add(thisMarble);
                previousEdgeMillimeters = thisPositionMillimeters + Constants.MarbleRadiusMillimeters;
                if (i > 0)
                {
                    thisMarble.PreviousMarble = _marbles[i - 1];
                    thisMarble.PreviousMarble.NextMarble = thisMarble;
                }
            }

            Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "marbles.txt");
            WriteMarbles();

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

            // Check collisions for all middle marbles
            UInt64 minDistanceForCollisions = UInt64.MaxValue;
            for (int i = 0; i < lastMarbleIndex; i++)
            {
                if (minDistance == 0)
                {
                    break;
                }

                UInt64 distance = _marbles[i].DistanceToCollision(_marbles[i].NextMarble);
                minDistanceForCollisions = Math.Min(minDistanceForCollisions, distance);
            }
            minDistanceForCollisions /= 2;

            minDistance = Math.Min(minDistanceForCollisions, minDistance);

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

        public int GetTotalDistanceOfMillimeters()
        {
            Marble thisMarble = _marbles[_checkMarble];
            while (!_marbles[_checkMarble].FellOut)
            {
                CleanupFallenMarbles(_checkMarble + 1);
                Step();
            }

            return (int)_marbles[_checkMarble].TravelDistanceMillimeters;
        }

        private void CleanupFallenMarbles(int dontCareIndex)
        {
            for (int i = _marbles.Count - 1; i >= 0; i--)
            {
                if (_marbles[i].FellOut)
                {
                    _marbles.RemoveAt(i);
                    _currentFellout++;
                    if (_currentFellout % 1000 == 0)
                    {
                        WriteMarbles();
                        Console.WriteLine(DateTime.Now.ToString());
                    }
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
