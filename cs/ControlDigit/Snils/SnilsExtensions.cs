using System;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var weightedSum = CalculateWeightedDigitSum(number);

            while (weightedSum > 0)
            {
                if (weightedSum < 100) return weightedSum;
                if (weightedSum == 100 || weightedSum == 101) return 0;
                weightedSum %= 101;
            }

            return 0;
        }


        private static int CalculateWeightedDigitSum(long number)
        {
            var weightedSum = 0;

            var currentNumber = 0;

            while (number > 0)
            {
                currentNumber += 1;
                weightedSum += ((int)number % 10) * currentNumber;
                number /= 10;
            }

            return weightedSum;
        }
    }
}
