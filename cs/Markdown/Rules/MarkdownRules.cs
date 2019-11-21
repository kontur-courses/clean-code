using System.Linq;
using Markdown.Extensions;

namespace Markdown.Rules
{
    public class MarkdownRules : IRules
    {
        private const char EscapeSymbol = '\\';

        public bool IsSeparatorValid(string text, int position, bool isFirst, int separatorLength)
        {
            if (text[position] == EscapeSymbol)
                return true;
            var anyNonDigitAround = text.GetNeighborsOfSubstring(position, position + separatorLength - 1)
                .Any(s => !char.IsDigit(s));
            return anyNonDigitAround && (isFirst
                       ? IsBeginSeparatorValid(text, position, separatorLength)
                       : IsEndSeparatorValid(text, position, separatorLength));
        }

        public bool IsSeparatorValid(string text, int position, bool isFirst, int separatorLength,
            string parentSeparator)
        {
            if (position < text.Length - 1 && text.Substring(position, 2) == "__" && parentSeparator == "_")
            {
                return false;
            }

            return IsSeparatorValid(text, position, isFirst, separatorLength);
        }

        private bool IsBeginSeparatorValid(string text, int position, int separatorLength)
        {
            return position < text.Length - separatorLength && !char.IsWhiteSpace(text[position + separatorLength]);
        }

        private bool IsEndSeparatorValid(string text, int position, int separatorLength)
        {
            return position > 0 && !char.IsWhiteSpace(text[position - 1]);
        }
    }
}