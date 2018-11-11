using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var sum = CountSum(number);
            do
            {
                if (sum < 100)
                    return (int)sum;
                if (sum == 100 || sum == 101)
                    return 0;
                sum %= 101;
            } while (true);
        }

        private static long CountSum(long number)
        {
            //var parity = new[] { new List<long>(), new List<long>() };
            var digits = new List<long>();
            //var currentParity = 0;
            var count = 1;
            while (number != 0)
            {
                var digit = number % 10;
                digits.Add(digit * count);
                number /= 10;
                count++;
                //currentParity = (currentParity + 1) % 2;
            }

            //for (var i = 0; i < parity[0].Count; i++)
            //{
            //    parity[0][i] *= 3;
            //}

            return digits.Sum();
        }
    }
}
