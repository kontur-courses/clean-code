using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var sum = CountSum(number);
            var M = sum % 10;
            if (M == 0)
                return 0;
            return (int)(10 - M);
        }

        private static long CountSum(long number)
        {
            var digits = Long.ToDigitsArray(number).ToArray();
            for (var i = digits.Length - 1; i >= 0; i -= 2)
            {
                digits[i] *= 3;
            }

            return digits.Sum();
        }
    }
}
