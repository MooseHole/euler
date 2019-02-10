using System;
using System.IO;

namespace Euler653
{
    class Program
    {
        static void Main(string[] args)
        {
            bool argsNumericOk = args.Length == 3;
            bool argsFileOk = args.Length == 1;

            UInt64 L = 0;
            int N = 0;
            int j = 0;


            if (argsNumericOk && !UInt64.TryParse(args[0], out L))
            {
                argsNumericOk = false;
            }
            if (argsNumericOk && !int.TryParse(args[1], out N))
            {
                argsNumericOk = false;
            }
            if (argsNumericOk && !int.TryParse(args[2], out j))
            {
                argsNumericOk = false;
            }

            Tube tube = null;
            if (argsNumericOk)
            {
                tube = new Tube(L, N, j);
            }
            else if (argsFileOk)
            {
                tube = new Tube(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]));
            }

            if (!argsNumericOk && !argsFileOk)
            {
                Console.WriteLine("Usage: " + System.AppDomain.CurrentDomain.FriendlyName + " L N j.  See https://projecteuler.net/problem=653");
                Console.WriteLine("Usage: " + System.AppDomain.CurrentDomain.FriendlyName + " filename.txt");
                return;
            }

            Console.WriteLine(tube?.GetTotalDistanceOfMillimeters());
        }
    }
}
