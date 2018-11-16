using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        private static int GetDigitSum(IEnumerable<int> digits, Func<int, int, int> multiplier)
        {
            var sum = 0;
            var index = 1;
            foreach (var digit in digits)
            {
                sum += multiplier(digit, index);
                index++;
            }

            return sum;
        }

        private static IEnumerable<int> GetDigits(long number)
        {
            return number
                .ToString()
                .Reverse()
                .Select(c => c - '0');
        }

        public static int CalculateSnils(this long number)
        {
            var digits = GetDigits(number);
            var sum = GetDigitSum(digits, (digit, index) => digit * index);

            if (sum < 100)
                return sum;
            if (sum < 102)
                return 0;

            var remainder = sum % 101;

            return remainder < 100 ? remainder : 0;
        }
    }
}
