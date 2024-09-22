using System;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var sum = GetLongNumbersSum(number);
            return GetSnilsControlDigit(sum);
        }
        public static int GetLongNumbersSum(long number)
        {
            var index = 1;

            var sum = 0;
            while (number > 0)
            {
                var currNumber = (int)number % 10;
                sum += currNumber * index;
                number /= 10;
                index++;
            }
            return sum;
        }

        public static int GetSnilsControlDigit(int sum)
        {
            if (sum < 100) return sum;
            if (sum == 100 || sum == 101)
                return 0;
            return GetSnilsControlDigit(sum %101 );
        }
    }

}
