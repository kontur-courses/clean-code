using System;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var numberToString = number.ToString();
            var summ = 0;
            for(int i = numberToString.Length - 1;i>=0;i-=2){
                summ += int.Parse(numberToString[i].ToString()) * 3;
            }
            for(int i = numberToString.Length - 2;i>=0;i-=2){
                summ += int.Parse(numberToString[i].ToString());
            }
            var M = (summ % 10 == 0)? 0: 10 - summ % 10;
            return M;
        }
    }
}
