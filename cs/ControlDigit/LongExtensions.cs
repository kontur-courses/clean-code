using System.Collections;
using System.Collections.Generic;

namespace ControlDigit
{
    public static class Long
    {
        public static IEnumerable<int> ToDigitsArray(long number)
        {
            while (number != 0)
            {
                yield return (int)number % 10;
                number /= 10;
            }
        }
    }
}