using System;
using System.Linq;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            int sum = 0;
            int factor = 3;
            do
            {
                int digit = (int)(number % 10);
                sum += factor * digit;
                factor = 4 - factor;
                number /= 10;
            } while (number > 0);

            int m = sum % 10;
            if (m == 0)
                return 0;
            return 10 - m;
        }
    }
}
