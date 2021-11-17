using System.Collections.Generic;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var sum = 0;
            var digitPosition = 1;

            foreach (var digit in DigitParser.GetDigitsFromEnd(number))
            {
                sum += digitPosition % 2 == 0 ? digit : digit * 3;
                digitPosition++;
            }

            var m = sum % 10;

            return m == 0 ? 0 : 10 - m;
        }
    }

    internal static class DigitParser
    {
        public static IEnumerable<byte> GetDigitsFromEnd(this long number)
        {
            while (number != 0)
            {
                var digit = (byte)(number % 10);
                number /= 10;
                yield return digit;
            }
        }
    }
}
