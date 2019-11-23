using System;
using System.Linq;
using Markdown.Extensions;

namespace Markdown.Rules
{
    public class MarkdownRules : IRules
    {
        private const string EscapeSymbol = "\\";

        public bool IsSeparatorValid(string text, int position, bool isFirst, string separator)
        {
            if (position >= text.Length || position < 0)
                throw new ArgumentException($"position {position} is not in string with length {text.Length}");

            if (separator == Environment.NewLine || separator.StartsWith(EscapeSymbol))
                return true;

            if (separator.StartsWith("#"))
                return IsHeaderSeparatorValid(text, position);

            if (separator.StartsWith("_"))
                return IsUnderscoreSeparatorValid(text, position, isFirst, separator);

            throw new NotImplementedException($"separator not supported {separator}");
        }

        public bool IsSeparatorValid(string text, int position, bool isFirst, string separator, string parentSeparator)
        {
            if (separator == "__" && parentSeparator == "_")
                return false;
            return IsSeparatorValid(text, position, isFirst, separator);
        }

        public bool IsSeparatorPaired(string separator)
        {
            return separator.StartsWith("_") || separator.StartsWith("#") || separator == Environment.NewLine;
        }

        public bool IsSeparatorPairedFor(string firstSeparator, string secondSeparator)
        {
            if (!IsSeparatorPaired(firstSeparator) || !IsSeparatorPaired(secondSeparator))
                throw new ArgumentException($"{firstSeparator} or {secondSeparator} is not paired");

            if (firstSeparator.StartsWith("#"))
                return secondSeparator == Environment.NewLine;

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
            return position == 0 || text[position - 1] == Environment.NewLine.Last();
        }

        private bool IsUnderscoreSeparatorValid(string text, int position, bool isFirst, string separator)
        {
            var anyNonDigitAround = text.GetNeighborsOfSubstring(position, position + separator.Length - 1)
                .Any(s => !char.IsDigit(s));
            return anyNonDigitAround && (isFirst
                       ? IsUnderscoreBeginSeparatorValid(text, position, separator)
                       : IsUnderscoreEndSeparatorValid(text, position));
        }

        private bool IsUnderscoreBeginSeparatorValid(string text, int position, string separator)
        {
            return position < text.Length - separator.Length && !char.IsWhiteSpace(text[position + separator.Length]);
        }

        private bool IsUnderscoreEndSeparatorValid(string text, int position)
        {
            return position > 0 && !char.IsWhiteSpace(text[position - 1]);
        }
    }
}