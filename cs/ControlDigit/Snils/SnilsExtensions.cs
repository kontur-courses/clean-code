using System;

namespace ControlDigit
{
    public static class SnilsExtensions
    {
        private const int ModuloBase = 10;
        private const int MaxValidSum = 100;
        private const int ModuloSnils = 101;

        public static int CalculateSnils(this long number)
        {
            int totalSum = 0;
            int placeValue = 1;
            
            long currentNumber = Math.Abs(number);

            while (currentNumber > 0)
            {
                int digit = (int)(currentNumber % ModuloBase);
                totalSum += digit * placeValue;
                
                currentNumber /= ModuloBase;
                placeValue++;
            }

            if (totalSum < MaxValidSum)
            {
                return totalSum;
            }

            if (totalSum == MaxValidSum || totalSum == MaxValidSum + 1)
            {
                return 0;
            }

            int remainder = totalSum % ModuloSnils;
            
            return remainder == MaxValidSum ? 0 : remainder;
        }
    }
}