using System;
using System.Diagnostics;
using NUnit.Framework;

namespace ControlDigit
{
    [TestFixture]
    public class Upc_PerformanceTests
    {
        public static int CalculateUpcFast(long number)
        {
            int sum = 0;
            int factor = 3;
            do
            {
                int digit = (int)(number % 10);
                sum += factor * digit;
                factor = 4 - factor;
                number /= 10;
            }
            while (number > 0);
            int m = sum % 10;
            if (m == 0)
                return 0;
            return 10 - m;
        }

        [Test]
        public void UpcPerformance()
        {
            var count = 10000000;
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                CalculateUpcFast(12345678L);
            Console.WriteLine("DoWhile " + sw.Elapsed);
            sw.Restart();
            for (int i = 0; i < count; i++)
                12345678L.CalculateUpc();
            Console.WriteLine("CleanCode " + sw.Elapsed);
        }
    }
}