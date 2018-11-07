using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ControlDigit
{
    public static class Helpers
    {
        public static IEnumerable<int> GetReversedDigitsEnumerable(long number)
        {
            do
            {
                yield return (int)(number % 10);
                number /= 10;
            }
            while (number > 0);
        }
    }
}