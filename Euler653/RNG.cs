using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler653
{
    class RNG
    {
        private List<UInt64> RValues;
        private const UInt64 FirstValue = 6_563_116;
        private const UInt64 ModValue = 32_745_673;

        public RNG()
        {
            RValues = new List<UInt64>
            {
                FirstValue
            };
        }

        public UInt64 R(int n)
        {
            if ((RValues.Count - 1) >= n)
            {
                return RValues[n];
            }

            UInt64 prev = R(n - 1);
            UInt64 thisValue = (prev * prev) % ModValue;
            RValues.Add(thisValue);
            return thisValue;
        }

        public UInt64 GetInitialGap(int n)
        {
            return (R(n) % 1000) + 1;
        }
    }
}
