using System;
using System.Collections.Generic;
using System.Linq;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var oddPositions = new List<int>();
            var evenPositions = new List<int>();
            bool evenPos = false;
            int digit;
            while (number != 0)
            {
                digit = (int)(number % 10);
                if(evenPos)
                    evenPositions.Add(digit);
                else
                    oddPositions.Add(digit);
                number /= 10;
                evenPos = !evenPos;
            }
            var result = 3 * oddPositions.Sum() + evenPositions.Sum();
            var M = result % 10;
            return M == 0 ? 0 : 10 - M;
        }
    }
}
