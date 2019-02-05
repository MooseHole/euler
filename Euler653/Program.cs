using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler653
{
    class Program
    {
        static void Main(string[] args)
        {
            bool argsOk = args.Length == 3;

            UInt64 L = 0;
            int N = 0;
            int j = 0;

            if (argsOk && !UInt64.TryParse(args[0], out L))
            {
                argsOk = false;
            }
            if (argsOk && !int.TryParse(args[1], out N))
            {
                argsOk = false;
            }
            if (argsOk && !int.TryParse(args[2], out  j))
            {
                argsOk = false;
            }

            if (!argsOk)
            {
                Console.WriteLine("Usage: " + System.AppDomain.CurrentDomain.FriendlyName + " L N j.  See https://projecteuler.net/problem=653");
                return;
            }

            Tube tube = new Tube(L, N);
            Console.WriteLine(tube.GetTotalDistanceOfMillimeters(j));
        }
    }
}
