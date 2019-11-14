using System;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var strNumber = number.ToString();
            return 1;
            //throw new NotImplementedException();
        }
    }
}
