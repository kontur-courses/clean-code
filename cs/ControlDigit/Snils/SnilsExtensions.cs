using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var reverseDigits = number.SplitToDigitsReverse(); // used from Upc
            var sum = GetSum(reverseDigits,
                (digit, i) => digit * (i + 1));
            return GetControlNumberBySnilsSum(sum);
        }

        public static long GetSum(IEnumerable<int> reverseDigits, Func<int, int, int> actionForDigitByIndex)
        {
            return reverseDigits
                .Select(actionForDigitByIndex)
                .Sum();
        }

        private static int GetControlNumberBySnilsSum(long snilsSum)
        {
            if (snilsSum < 100)
                return (int)snilsSum;
            return snilsSum <= 101
                ? 0
                // ReSharper disable once TailRecursiveCall
                : GetControlNumberBySnilsSum(snilsSum % 101);
        }
    }
}
