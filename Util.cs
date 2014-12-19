using System;
using System.Threading;

namespace BScan
{
    class Util
    {
        public static int GetRandom(int lower, int upper)
        {
            if (lower >= (upper-1))
                return lower;
            Random rnd = new Random();
            return rnd.Next(upper - lower) + lower;
        }
        public static void Wait(int second)
        {
            Console.WriteLine("Waiting {0} seconds...", second);
            Thread.Sleep(second * 1000);
        }
        public static void Wait(int minSecond, int maxSecond)
        {
            int second = GetRandom(minSecond, maxSecond);
            Wait(second);
        }
    }
}
