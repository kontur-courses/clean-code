using System;
using System.Diagnostics;
using System.Linq;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var digits = SnilsExtensions.GetReversedDigits(number);

            int NumByPosition((int position, int digit) tuple) => ((tuple.position % 2 == 1 ? 3 : 1) * tuple.digit);

            var sum = SnilsExtensions.GetSum(NumByPosition, digits);

            var m = sum % 10;
            return m == 0 ? 0 : 10 - m;
        }
    }
}