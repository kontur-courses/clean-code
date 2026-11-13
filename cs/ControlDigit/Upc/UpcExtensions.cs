using System;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var factor = 3;
            var sum = 0L;
            var position = 0;

            do
            {
                var digit = number % 10;

                sum += position % 2 == 0 ? digit * factor : digit;

                number /= 10;
                position++;

            } while (number > 0);

            return (int)((10 - sum % 10) % 10);
        }
    }
}