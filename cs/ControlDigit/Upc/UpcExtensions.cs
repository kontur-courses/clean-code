using System;
using System.Linq;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var controlSum = Helpers.GetReversedDigitsEnumerable(number)
                .ToArray()
                .Select((d, i) => (i % 2 == 0) ? d * 3 : d)
                .Sum();
            
            var m = controlSum % 10;
            
            if (m != 0)
            {
                return (10 - m);
            }
            
            return 0;
        }
    }
}
