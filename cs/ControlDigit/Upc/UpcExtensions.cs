using System;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var sum = GetSum(number, (pos) => { return (pos % 2 == 0) ? 1 : 3; });

            if (sum % 10 == 0)
                return 0;
            return (int)(10 - sum % 10);
            
        }

        private static long GetSum(long number, Func<int,int> calculateFactorRule)
        {
            var sum = 0L;
            var count = 1;
            do
            {
                var digit = number % 10;
                sum += calculateFactorRule(count) * digit;
                count++;
                number /= 10;
            } while (number > 0);

            return sum;
        }
    }
}
 