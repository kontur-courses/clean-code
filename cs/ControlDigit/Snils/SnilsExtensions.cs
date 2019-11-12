using System;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var controlSum = GetControlSum(number.ToString());
            return GetControlNum(controlSum);
        }

        private static int GetControlSum(string number)
        {
            var result = 0;
            for (int i = 0; i < number.Length; ++i)
                result += int.Parse(number[i].ToString()) * (number.Length - i);
            return result;
        }

        private static int GetControlNum(int controlSum)
        {
            if (controlSum > 101)
                controlSum %= 101;
            if (controlSum < 100)
                return controlSum;
            else
                return 0;
        }
    }
}
