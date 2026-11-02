using System;
using System.Collections.Generic;

namespace ControlDigit
{
    public static class LongExtensions
    {
        public static IEnumerable<int> GetDigits(this long number)
        {
            while (number > 0)
            {
                yield return (int)(number % 10);
                number /= 10;
            }
        }

        public static int SumByPosition(this long number, int startPosition, Func<int, int, int> positionFunction)
        {
            var sum = 0L;
            var position = startPosition;

            foreach (var digit in number.GetDigits())
            {
                sum += positionFunction(position, digit);
                position++;
            }

            return (int)sum;
        }
    }
}