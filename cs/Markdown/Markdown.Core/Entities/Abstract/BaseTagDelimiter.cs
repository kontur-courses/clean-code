using Markdown.Core.Extensions;

namespace Markdown.Core.Entities.Abstract
{
    public abstract class BaseTagDelimiter : BaseTag
    {
        protected abstract int DelimiterLength { get; }
        protected abstract string Prefix { get; }
        protected abstract string Postfix { get; }

        protected HashSet<string> Delimeters;

        public override Token? TryGetToken(string input, int startPos)
        {
            if (startPos + DelimiterLength >= input.Length)
                return null;

            var delimiter = GetDelimeter(input, startPos);

            if (!CheckPrefixCorrect(input, startPos, delimiter) ||
                !GetSuffixIndex(input, startPos, delimiter, out var suffIndex))
                return null;

            var strValue = input.Substring(startPos + DelimiterLength, suffIndex - DelimiterLength - startPos);
            return new Token(strValue, Prefix, Postfix, Priority, suffIndex - startPos + DelimiterLength, true);
        }

        private string? GetDelimeter(string input, int startPos)
        {
            var supposedDelimiter = input.Substring(startPos, DelimiterLength);
            return Delimeters.Contains(supposedDelimiter) ? supposedDelimiter : null;
        }

        private bool CheckPrefixCorrect(string input, int startPos, string? delimiter)
        {
            if (delimiter == null)
                return false;

            var leftBorder = input.GetLeftBorder(startPos, delimiter);
            var rightBorder = input.GetRightBorder(startPos, delimiter);

            return !input.IsEscapeSymbol(startPos) &&
                   !(delimiter.Contains("_") && input.IsInsideWord(leftBorder, rightBorder)) &&
                   input.IsNoSpaceAfter(rightBorder) &&
                   !input.IsPunctuationAfter(leftBorder, rightBorder);
        }

        private bool CheckSuffixCorrect(string input, int startPos, string? delimiter)
        {
            if (delimiter == null)
                return false;

            var leftBorder = input.GetLeftBorder(startPos, delimiter);
            var rightBorder = input.GetRightBorder(startPos, delimiter);

            return !input.IsEscapeSymbol(startPos) &&
                   input.IsNoSpaceBefore(leftBorder) &&
                   !IsEmptyValueTag(input, startPos, leftBorder, rightBorder) &&
                   !input.IsPunctuationBefore(leftBorder, rightBorder);
        }

        private bool GetSuffixIndex(string input, int startPos, string delimiter, out int suffIndex)
        {
            var nestedTagCount = 0;
            suffIndex = input.IndexOf(delimiter, startPos + DelimiterLength, StringComparison.Ordinal);

            while (suffIndex != -1)
            {
                if (CheckSuffixCorrect(input, suffIndex, delimiter))
                {
                    if (nestedTagCount == 0)
                        return true;

                    nestedTagCount--;
                    suffIndex = input.IndexOf(delimiter, suffIndex + DelimiterLength, StringComparison.Ordinal);
                    continue;
                }

                if (CheckPrefixCorrect(input, suffIndex, delimiter))
                {
                    nestedTagCount++;
                    suffIndex = input.IndexOf(delimiter, suffIndex + DelimiterLength, StringComparison.Ordinal);
                    continue;
                }

                suffIndex = input.IndexOf(delimiter, suffIndex + 1, StringComparison.Ordinal);
            }

            return false;
        }

        private bool IsEmptyValueTag(string input, int startPos, int leftBorder, int rightBorder)
        {
            return rightBorder - leftBorder == DelimiterLength * 2 &&
                   leftBorder == startPos &&
                   input.IsInsideWord(leftBorder, rightBorder);
        }
    }
}
