using System;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var stringedNum = number.ToString();
            var sum = 0;
            for (var i = 0; i < stringedNum.Length; i++)
            {
                var curDigit = stringedNum[i] - '0';

                if (i % 2 != 0)
                    sum += curDigit * 3;
                else
                    sum += curDigit;

            }

            var controlDigit = 0;
            var remainder = sum % 10;
            if (remainder != 0)
                controlDigit = 10 - remainder;
            return controlDigit;
        }
    }
}
