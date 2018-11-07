using System;

namespace ControlDigit
{
    public static class Isbn13Extensions
    {
        public static int CalculateIsbn13(this long number)
        {
            int sum = 0;
            int factor = 1;
            do
            {
                int digit = (int)(number % 10);
                sum += factor * digit;
                factor = 4 - factor;
                number /= 10;
            }
            while (number > 0);
            int m = sum % 10;
            if (m == 0)
                return 0;
            return 10 - m;
        }
    }
}
