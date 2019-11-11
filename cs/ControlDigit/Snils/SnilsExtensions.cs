using System;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var sum = 0;
            var count = 1;
            var numberString = number.ToString();
            for (var i = numberString.Length - 1; i >= 0; i--)
                sum += GetPreparedDigit(numberString[i], count++);
            return GetControlDigit(sum);
        }

        private static int GetControlDigit(int sum)
        {
            while (true)
            {
                if (sum < 100) return sum;
                if (sum == 100 || sum == 101) return 0;
                sum = sum % 101;
            }
        }

        private static int GetPreparedDigit(char digit, int index)
        {
            return int.Parse(digit.ToString()) * index;
        }
    }
}