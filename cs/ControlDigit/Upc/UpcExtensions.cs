using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ControlDigit
{
    public static class UpcExtensions
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

        public static int CalculateUpc(this long number)
        {
            var digits = GetDigits(number);
            var sum = GetDigitSum(digits, (digit, index) => digit * (index % 2 == 1 ? 3 : 1));

            var remainder = sum % 10;

            return remainder == 0 ? 0 : 10 - remainder;
        }
    }
}
