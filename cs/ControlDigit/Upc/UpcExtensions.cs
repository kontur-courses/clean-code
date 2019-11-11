using System;
using System.Linq;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var controlSum = 0;
            var numberString = number.ToString();
            controlSum += CalculateOddPositions(numberString) * 3;
            controlSum += CalculateEvenPositions(numberString);
            var controlNumber = controlSum % 10;
            return controlSum % 10 == 0 ? controlNumber : 10 - controlNumber;
        }

        private static int CalculateEvenPositions(string numberString)
        {
            var result = 0;
            for (var i = numberString.Length - 2; i >= 0; i -= 2)
                result += numberString[i] - '0';
            return result;
        }

        private static int CalculateOddPositions(string numberString)
        {
            var result = 0;
            for (var i = numberString.Length - 1; i >= 0; i -= 2)
                result += numberString[i] - '0';
            return result;
        }
    }
}