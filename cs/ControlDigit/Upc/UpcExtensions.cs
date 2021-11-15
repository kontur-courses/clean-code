namespace ControlDigit
{
    public static class UpcExtensions
    {
        public static int CalculateUpc(this long number)
        {
            var sum = 3 * MathExtensions.OddPositionsDigitsSum(number)
                + MathExtensions.EvenPositionsDigitsSum(number);

            return MathExtensions.CalculateSubstructedModulo(sum, 10);
        }
    }
}
