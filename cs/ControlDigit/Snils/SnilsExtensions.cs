using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var digits = GetReversedDigits(number);
            var sum = GetSum(tuple => tuple.digit * tuple.position, digits);

            return GetCutted(100, 101, sum);
        }

        public static int GetSum(Func<(int position, int digit), int> multipleByPosition, IEnumerable<int> digits) 
            => digits.Select((t, i) => multipleByPosition((i + 1, t))).Sum();

        public static IEnumerable<int> GetReversedDigits(long number) 
            => number.ToString().Reverse().Select(n => n -  '0');

        public static int GetCutted(int min, int max, int sum)
        {
            if (sum > max) sum %= max;
            return sum < min ? sum : 0;
        }
    }
    
}