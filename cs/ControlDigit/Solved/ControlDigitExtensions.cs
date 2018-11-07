using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace ControlDigit.Solved
{
    public static class ControlDigitExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var sum = number
                          .GetDigitsFromLeastSignificant()
                          .SumWithWeights(Enumerable.Range(1, 9)) % 101;
            return sum == 100 ? 0 : sum;
        }

        public static int CalculateUpcOld(this long number)
        {
            int sum = 0;
            int factor = 3;
            do
            {
                int digit = (int)(number % 10);
                sum += factor * digit;
                factor = 4 - factor;
                number /= 10;

            }
            while (number > 0);

            int m = sum % 10;
            if (m == 0)
                return 0;
            return 10 - m;
        }

        private static readonly int[] weights = new[] { 3, 1 }.Repeat().Take(11).ToArray();

        public static int CalculateUpc(this long number)
        {
            var m = number
                .GetDigitsFromLeastSignificant()
                .SumWithWeights(weights) % 10;
            return m == 0 ? 0 : 10 - m;
        }
    }

    // Общий код, ничего не знает про контрольные разряды и прикладную задачу.
    public static class MoreEnumerableExtensions
    {
        public static IEnumerable<int> GetDigitsFromLeastSignificant(this long number)
        {
            do
            {
                yield return (int)(number % 10);
                number /= 10;
            } while (number > 0);
        }

        public static int SumWithWeights(this IEnumerable<int> numbers, IEnumerable<int> weights)
        {
            return numbers.Zip(weights, (n, w) => n * w).Sum();
        }

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> items)
        {
            var i2 = items.ToList();
            while (true)
                foreach (var item in i2) yield return item;
            // ReSharper disable once FunctionNeverReturns
        }
    }
}
