using System;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        public static int CalculateSnils(this long number)
        {
            var digits = GetDigitsArray(9, number);
            var koef = new int[9];
            for (int i = 0,j = 9; i < koef.Length; i++,j--)
                koef[i] = j;
            var sum = GetSummary(digits, koef);
            return GetControlDigit(sum);
        }

        public static int[] GetDigitsArray(int arrayLength, long number)
        {
            var str = number.ToString();
            var array = new int[arrayLength];
            for (int i = str.Length - 1, j = arrayLength-1; i >= 0; i--, j--)
            {
                array[j] = Int32.Parse(str[i].ToString());
            }
            return array;
        }

        public static int GetSummary(int[] array, int[] koef)
        {
            var sum = 0;
            for (int i = 0; i < array.Length; i++)
                sum+= koef[i] * array[i];
            return sum;
        }

        public static int GetControlDigit(int num)
        {
            if (num == 100 || num == 101)
                return 0;
            else if (num < 100)
                return num;
            else
                return GetControlDigit(num % 101);
        }
    }
}
