using System.Collections.Generic;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var digits = number.SplitToDigitsReverse();
            var sum = StepOne(digits);
            var m = (int) (sum % 10);
            return m == 0 ? 0 : 10 - m;
        }

        public static IEnumerable<int> SplitToDigitsReverse(this long number)
        {
            while (number > 0)
            {
                yield return (int)(number % 10);
                number /= 10;
            }
        }

        private static long StepOne(IEnumerable<int> digits)
        {
            return SnilsExtensions.GetSum(digits,
                    (digit, i) => digit * (i % 2 == 0 ? 3 : 1));
        }
    }
}
