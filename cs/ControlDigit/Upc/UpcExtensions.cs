using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var M = number.GetReversedDigits().Select((x, y) => y % 2 == 0 ? x * 3 : x).Sum();
            if (M %10== 0)
            {
                return 0;
            }

            return (10 - M%10);
        }

        public static IEnumerable<int> GetReversedDigits(this long number)
        {
            return number.ToString()
                .Reverse()
                .Select(t => (int)Convert.ToInt64(t.ToString()));
        }
    }
}
