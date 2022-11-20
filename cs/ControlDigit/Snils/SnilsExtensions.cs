using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var result = (int)GetWeightedSum(number, i => i);
            while (true)
            {
                if (result < 100)
                    return result;
                if (result == 100 
                    || result == 101)
                    return 0;
                if(result > 101)
                    result %= 101;
            }
        }

        public static long GetWeightedSum(long number, Func<int, int> weightByIndex) =>
        number.GetDigitsOfNumber()
                .Select((number, index) => number * weightByIndex(index+1))
                .Sum();

        public static int[] GetDigitsOfNumber(this long number)
        {
            var digits = new List<int>();
            do
            {
                var digit = number % 10;
                digits.Add((int)digit);
                number /= 10;
            }while (number > 0);

            return digits.ToArray();
        }
    }
}
