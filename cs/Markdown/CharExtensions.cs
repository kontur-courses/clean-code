namespace Markdown
{
    public static class CharExtensions
    {
        public static bool IsLetterOrDigitOrSpecifiedChar(this char testedChar, char specifiedChar) =>
            char.IsLetterOrDigit(testedChar) || testedChar == specifiedChar;
    }
}
