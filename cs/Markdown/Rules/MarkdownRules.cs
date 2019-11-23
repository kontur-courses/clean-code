using System;
using System.Linq;
using Markdown.Extensions;

namespace Markdown.Rules
{
    public class MarkdownRules : IRules
    {
        private const char EscapeSymbol = '\\';

        public bool IsSeparatorValid(string text, int position, bool isFirst, int separatorLength)
        {
            if (position >= text.Length || position < 0)
                throw new ArgumentException($"position {position} is not in string with length {text.Length}");

            switch (text[position])
            {
                case EscapeSymbol:
                    return true;
                case '_':
                    return IsUnderscoreSeparatorValid(text, position, isFirst, separatorLength);
                case '#':
                    return IsHeaderSeparatorValid(text, position);
                case '\n':
                    return true;
                default:
                    throw new NotImplementedException($"separator not supported {text[position]}");
            }
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

        public bool IsSeparatorPaired(string separator)
        {
            return separator.StartsWith("_") || separator.StartsWith("#") || separator == "\n";
        }

        public bool IsSeparatorPairedFor(string firstSeparator, string secondSeparator)
        {
            if (!IsSeparatorPaired(firstSeparator) || !IsSeparatorPaired(secondSeparator))
                throw new ArgumentException($"{firstSeparator} or {secondSeparator} is not paired");

            if (firstSeparator.StartsWith("#"))
                return secondSeparator == "\n";

            if (firstSeparator.StartsWith("_"))
                return firstSeparator == secondSeparator;
            return false;
        }

        public bool IsSeparatorOpening(string separator)
        {
            return separator.StartsWith("_") || separator.StartsWith("#");
        }

        private bool IsHeaderSeparatorValid(string text, int position)
        {
            return position == 0 || text[position - 1] == '\n';
        }

        private bool IsUnderscoreSeparatorValid(string text, int position, bool isFirst, int separatorLength)
        {
            var anyNonDigitAround = text.GetNeighborsOfSubstring(position, position + separatorLength - 1)
                .Any(s => !char.IsDigit(s));
            return anyNonDigitAround && (isFirst
                       ? IsUnderscoreBeginSeparatorValid(text, position, separatorLength)
                       : IsUnderscoreEndSeparatorValid(text, position));
        }

        private bool IsUnderscoreBeginSeparatorValid(string text, int position, int separatorLength)
        {
            return position < text.Length - separatorLength && !char.IsWhiteSpace(text[position + separatorLength]);
        }

        private bool IsUnderscoreEndSeparatorValid(string text, int position)
        {
            return position > 0 && !char.IsWhiteSpace(text[position - 1]);
        }
    }
}