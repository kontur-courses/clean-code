using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var result = GetDigits(number).WieghtedSum(x => x + 1);
            return CalculateControlSum(result);
        }

        public static int WieghtedSum(this IEnumerable<int> enumerable, Func<int, int> weightFunc)
        {
            int nDigit = 0;
            return enumerable.Aggregate(0, (sum, x) =>
            {
                sum += x * weightFunc(nDigit);
                nDigit++;
                return sum;
            });
        }

        public static IEnumerable<int> GetDigits(long number)
        {
            while (number > 0)
            {
                yield return (int)(number % 10);
                number /= 10;
            }
        }

        public static int CalculateControlSum(int sum)
        {
            if (sum < 100)
                return sum;
            if (sum == 100 || sum == 101)
                return 0;
            return CalculateControlSum(sum % 101);
        }
        
        

    }
}
