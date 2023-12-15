using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var length = number.ToString().Length;
            var sum = number.GetReversedDigits().Select((x, y) => x * (y+1)).Sum();
            
            if (sum < 100) return sum;
            if (sum == 100 || sum == 101) return 0;
            var res = sum % 101;
            return res == 100 ? 0 : res;
        }
    }
}
