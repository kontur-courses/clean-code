using System;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var sum = MathExtensions.DigitsSumModified(number, SnilsModifier);

            return CalculateSnilsControlDigit(sum);
        }

        private static int CalculateSnilsControlDigit(int sum)
        {
            while (true)
            {
                if (sum < 100)
                    return sum;
                else if (sum > 101)
                {
                    sum = sum % 101;
                }
                else
                    return 0;
            }
        }

        private static int SnilsModifier(int digit, int index)
            => digit * (index + 1);
    }
}
