using System;
using System.Linq;

namespace ControlDigit
{
    public class MathExtensions
    {
        public static int OddPositionsDigitsSum(long number)
        {
            return DigitsSum(number, 2);
        }

        public static int EvenPositionsDigitsSum(long number)
        {
            return DigitsSum(number, 2, 1);
        }

        public static int DigitsSumModified(long number, Func<int, int, int> modifier)
        {
            return DigitsSum(number, 1, 0, modifier);
        }

        public static int DigitsSum(long number, int iterationIncrement = 1,
            int startIndex = 0, Func<int, int, int> modifier = null)
        {
            var sum = 0;
            var numStr = number.ToString().Reverse().ToArray();
            for (var i = startIndex; i < numStr.Length; i += iterationIncrement)
            {
                var digit = int.Parse(numStr[i].ToString());
                sum += modifier == null ? digit : modifier(digit, i);
            }
            return sum;
        }

        public static int CalculateSubstructedModulo(int number, int radix)
        {
            var result = number % radix;
            if (result != 0)
                result = radix - result;
            return result;
        }
    }
}
