using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var result = GetDigits(number).WieghtedSum(x => x % 2 == 1 ? 1 : 3);
            
            if (result % 10 != 0)
                return 10 - (result % 10);
            return 0;
        }
        
        public static IEnumerable<int> GetDigits(long number)
        {
            while (number > 0)
            {
                yield return (int)(number % 10);
                number /= 10;
            }
        }
    }
}
