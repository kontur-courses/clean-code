using System;
using ControlDigit;
namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var digits = SnilsExtensions.GetDigitsArray(11, number);
            var koef = new int[11];
            for (int i = 0; i < koef.Length; i++)
                koef[i] = (int)Math.Pow(3, (i+1) % 2);
            var sum = SnilsExtensions.GetSummary(digits, koef);
            return sum % 10 == 0 ? 0 : 10 - (sum % 10);
        }
    }
}
