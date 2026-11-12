using System;

namespace ControlDigit
{
    public static class UpcExtensions
    {
        private const int OddPositionMultiplier = 3;
        private const int EvenPositionMultiplier = 1;
        private const int Base = 10;

        public static int CalculateUpc(this long number)
        {
            
            int sum = 0;
            bool isOddPosition = true;

            while (number > 0)
            {
                int digit = (int)(number % Base);
                
                int multiplier = isOddPosition ? OddPositionMultiplier : EvenPositionMultiplier;
                sum += digit * multiplier;

                number /= Base;
                
                isOddPosition = !isOddPosition;
            }

            int m = sum % Base;

            return m == 0 ? 0 : Base - m;
        }
    }
}