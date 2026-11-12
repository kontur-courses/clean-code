using System;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var stringNumber = number.ToString().PadLeft(11,'0');
            
            var length = stringNumber.Length;

            var weightedSum = 0;

            for (var i = length; i >= 1; i--)
            {
                if (i % 2 == 0)
                {
                    weightedSum += (stringNumber[i - 1] - '0');
                }
                else
                {
                    weightedSum += 3 * (stringNumber[i - 1] - '0');
                }
            }
            
            var lastDigit = weightedSum % 10;

            if (lastDigit == 0)
            {
                return 0;
            }
            else
            {
                return 10 - lastDigit;
            }
        }
    }
}
