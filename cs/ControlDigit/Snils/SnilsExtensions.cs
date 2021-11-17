using System;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var sum = GetSumNumbersMultipliedByTheIndex(number);
            return GetControlSum(sum);
        }

        private static int GetSumNumbersMultipliedByTheIndex(long number)
        {
            return number.GetDigitsFromEnd()
                .Select((digit, index) => digit * (index + 1))
                .Sum();
        }

        private static int GetControlSum(int sum)
        {
            if (sum > 101)
                sum %= 101;

            return sum < 100 ? sum : 0;
        }
    }
}
