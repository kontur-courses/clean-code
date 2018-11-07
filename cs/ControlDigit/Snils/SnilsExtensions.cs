using System;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var controlSum = Helpers.GetReversedDigitsEnumerable(number)
                .Select((d, i) => d * (i + 1))
                .Sum();

            do
            {
                if (controlSum == 100 || controlSum == 101)
                {
                    return 0;
                }
                
                if (controlSum < 100)
                {
                    return controlSum;
                }

                controlSum %= 101;
            } while (controlSum >= 100);

            return controlSum;
        }
    }
}
