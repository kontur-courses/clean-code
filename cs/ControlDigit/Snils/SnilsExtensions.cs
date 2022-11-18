using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var weights = Enumerable.Range(1, 9).ToArray();

            var digits = ParseNumber(number);

            var sum = GetWeightedSum(digits, weights);

            var m = sum % 101;

            if (m != 100)
                return m;

            return 0;
        }

        private static int GetWeightedSum(IReadOnlyList<int> digits, int[] weights)
        {
            var digitsWeights = GetDigitsWeights(digits.Count, weights);

            return digits.Zip(digitsWeights, (d, w) => d * w).Sum();
        }

        private static IReadOnlyList<int> ParseNumber(long number)
        {
            var result = new List<int>();

            while (number > 0)
            {
                result.Add((int)(number % 10));

                number /= 10;
            }

            result.Reverse();

            return result.AsReadOnly();
        }

        private static IReadOnlyList<int> GetDigitsWeights(int numberLength, int[] weights)
        {
            var digitsWeights = new List<int>();

            for (int i = 0; i < numberLength; i++)
            {
                digitsWeights.Add(weights[i % weights.Length]);
            }

            digitsWeights.Reverse();

            return digitsWeights.AsReadOnly();
        }

    }
}
