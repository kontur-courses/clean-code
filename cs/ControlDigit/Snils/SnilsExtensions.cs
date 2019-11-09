using System;
using System.Linq;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        private static int tailSnils(int x)
        {
            if (x < 100)
                return x;

            if (x > 101)
                return tailSnils(x % 101);
            
            return 0;
        }
        
        public static int CalculateSnils(this long number)
        {
            return number.CalculateHash((i => i + 1), tailSnils);
        }
    }
}
