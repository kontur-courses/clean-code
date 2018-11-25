namespace Markdown
{
    public static class CharExtensions
    {
        public static bool IsLetterOrDigitOrSpecifiedChar(this char testedChar, char? specifiedChar = null)
        {
            return char.IsLetterOrDigit(testedChar) || specifiedChar.HasValue && testedChar == specifiedChar;
        }
    }
}
