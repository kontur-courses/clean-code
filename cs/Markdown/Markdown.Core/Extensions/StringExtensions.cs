namespace Markdown.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsInsideWord(this string input, int leftBorder, int rightBorder)
        {
            return leftBorder > 0 && char.IsLetterOrDigit(input[leftBorder - 1])
                                  && rightBorder != input.Length &&
                                  char.IsLetterOrDigit(input[rightBorder]);
        }

        public static bool IsNoSpaceAfter(this string input, int rightBorder)
        {
            return rightBorder != input.Length && !char.IsWhiteSpace(input[rightBorder]);
        }

        public static bool IsNoSpaceBefore(this string input, int leftBorder)
        {
            return leftBorder != 0 && !char.IsWhiteSpace(input[leftBorder - 1]);
        }

        public static bool IsPunctuationAfter(this string input, int leftBorder, int rightBorder)
        {
            return char.IsPunctuation(input[rightBorder]) && leftBorder != 0 &&
                   !char.IsWhiteSpace(input[leftBorder - 1]) && !char.IsPunctuation(input[leftBorder - 1]);
        }

        public static bool IsPunctuationBefore(this string input, int leftBorder, int rightBorder)
        {
            return char.IsPunctuation(input[leftBorder - 1]) && rightBorder != input.Length &&
                   !char.IsWhiteSpace(input[rightBorder]) &&
                   !char.IsPunctuation(input[rightBorder]);
        }

        public static bool IsEscapeSymbol(this string input, int pos)
        {
            return pos > 0 && input[pos - 1] == '\\';
        }

        public static int GetLeftBorder(this string input, int startPos, string delimiter)
        {
            var leftBorder = startPos;
            while (leftBorder >= 0 && input.IndexOf(delimiter, leftBorder, StringComparison.Ordinal) == leftBorder)
                leftBorder--;
            leftBorder++;

            return leftBorder;
        }

        public static int GetRightBorder(this string input, int startPos, string delimiter)
        {
            var rightBorder = startPos;
            while (input.IndexOf(delimiter, rightBorder, StringComparison.Ordinal) == rightBorder)
                rightBorder++;
            rightBorder--;

            return rightBorder + delimiter.Length;
        }
    }
}
