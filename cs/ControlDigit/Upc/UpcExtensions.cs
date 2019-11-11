using System;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var stringNumber = number.ToString();
            var oddSum = GetSumOfDigits(stringNumber, false);
            var evenSum = GetSumOfDigits(stringNumber, true);
            var sum = (oddSum * 3 + evenSum) % 10;
            return (10 - sum) % 10;
        }

        private static int GetSumOfDigits(string stringNumber, bool isEven)
        {
            var sum = 0;
            var offset = isEven ? 1 : 0;
            for (var i = stringNumber.Length - offset - 1; i >= 0;  i -= 2)
                sum += stringNumber[i] - '0';
            return sum;
        }
    }
}