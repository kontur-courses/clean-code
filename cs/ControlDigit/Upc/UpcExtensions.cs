using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            return number.CalculateHash( 
                i => i % 2 == 0 ? 3 : 1, 
                x => x % 10 == 0 ? 0 : 10 - x % 10);
        }

        public static int CalculateHash(this long number, Func<int, int> positionCoefficient, Func<int, int> trailOperation)
        {
            var digits = number.GetDigits().ToList();
            var hashSum = 0;
         
            for (var i = 0; i < digits.Count; ++i)
                hashSum += positionCoefficient(i) * digits[i];

            return trailOperation(hashSum);
        }

        public static IEnumerable<int> GetDigits(this long number)
        {
            if (number == 0)
                yield return 0;
            else
            {
                while (number > 0)
                {
                    yield return (int) (number % 10);
                    number /= 10;
                }
            }
        }
    }
}
