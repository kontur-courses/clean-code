using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var controlSum = number.SelectDigits((digit, position) => digit * position)
                .Sum();
            return FixControlSum(controlSum);
        }

        private static int FixControlSum(int checksum)
        {
            if (checksum < 100)
                return checksum;
            if (checksum == 100 || checksum == 101)
                return 0;
            return FixControlSum(checksum % 101);
        }

        private static IEnumerable<T> SelectDigits<T>(this long number, Func<int, int, T> selector)
        {
            var currentPosition = 1;
            var temp = number;
            do
            {
                var currentNumber = (int) temp % 10;
                yield return selector.Invoke(currentNumber, currentPosition++);
            } while ((temp /= 10) != 0);
        }
    }
}