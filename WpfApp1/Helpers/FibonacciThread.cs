﻿using System.Diagnostics;


namespace WpfApp1.Helpers
{
    internal static class FibonacciThread
    {
        public static (long result, long timeNs) Calculate(long num)
        {
            var sw = Stopwatch.StartNew();
            long a = 0, b = 1, temp;
            for (int i = 2; i <= num; i++)
            {
                temp = a + b;
                a = b;
                b = temp;
            }
            sw.Stop();
            return (num == 0 ? 0 : b, sw.ElapsedTicks * (1_000_000_000 / Stopwatch.Frequency));
        }
    }

}
