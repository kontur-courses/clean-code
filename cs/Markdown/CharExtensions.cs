namespace Markdown
{
    public static class CharExtensions
    {
        public static bool IsDigitOrWhiteSpace(this char symbol)
        {
            return char.IsDigit(symbol) || char.IsWhiteSpace(symbol);
        }
    }
}