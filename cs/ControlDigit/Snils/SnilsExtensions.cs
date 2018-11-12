using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var sum = GetSum(number, (pos) => { return pos; });
            while (sum > 101)
            {
                sum %= 101;
            }
            if (sum < 100)
                return (int)sum;    
            return 0;        
        }

        private static long GetSum(long number, Func<int, int> calculateFactorRule)
        {
            var sum = 0L;
            var count = 1;
            do
            {
                var digit = number % 10;
                sum += calculateFactorRule(count) * digit;
                count++;
                number /= 10;
            } while (number > 0);
            return sum;
        }

    }
}
