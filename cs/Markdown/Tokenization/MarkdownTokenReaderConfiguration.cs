using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tokenization
{
    public class MarkdownTokenReaderConfiguration : ITokenReaderConfiguration
    {
        private static readonly IEnumerable<int> HeaderSeparatorLengthRange = Enumerable.Range(2, 6);
        private const char EscapeSymbol = '\\';

        public bool IsSeparator(string text, int position)
        {
            CheckIfPositionIsCorrect(text, position);

            if (text[position] == EscapeSymbol)
            {
                return position < text.Length - 1 &&
                       (text[position + 1] == EscapeSymbol || IsSeparator(text, position + 1));
            }

            return IsHeaderBeginSeparator(text, position) || IsHeaderEndSeparator(text, position) ||
                   text[position] == '_';
        }

        public int GetSeparatorLength(string text, int position)
        {
            if (!IsSeparator(text, position))
                throw new ArgumentException($"there is no separator at position {position}");

            if (text[position] == EscapeSymbol)
            {
                return text[position + 1] == EscapeSymbol ? 2 : GetSeparatorLength(text, position + 1) + 1;
            }

            if (text[position] == '#')
                return GetHeaderBeginSeparatorLength(text, position);

            return IsHeaderEndSeparator(text, position)
                ? Environment.NewLine.Length
                : GetUnderscoreSeparatorLength(text, position);
        }

        public string GetSeparatorValue(string text, int position)
        {
            CheckIfPositionIsCorrect(text, position);
            return text.Substring(position, GetSeparatorLength(text, position));
        }

        private bool IsHeaderEndSeparator(string text, int position)
        {
            if (position == text.Length - 1)
            {
                return Environment.NewLine.Length == 1 && text[position] == Environment.NewLine[0];
            }

            return text.Substring(position, Environment.NewLine.Length) == Environment.NewLine;
        }

        private bool IsHeaderBeginSeparator(string text, int position)
        {
            return text[position] == '#' && HeaderSeparatorLengthRange
                       .Where(length => position + length < text.Length)
                       .Select(length => text.Substring(position, length))
                       .Any(s => s.Distinct().Count() == 2 && s.EndsWith(" "));
        }

        private void CheckIfPositionIsCorrect(string text, int position)
        {
            if (position >= text.Length || position < 0)
                throw new ArgumentException($"position {position} is not in string with length {text.Length}");
        }

        private int GetHeaderBeginSeparatorLength(string text, int position)
        {
            return HeaderSeparatorLengthRange
                .Where(length => position + length < text.Length)
                .Select(length => text.Substring(position, length))
                .First(s => s.Distinct().Count() == 2 && s.EndsWith(" ")).Length;
        }

        private int GetUnderscoreSeparatorLength(string text, int position)
        {
            return position == text.Length - 1 || text[position + 1] != '_' ? 1 : 2;
        }
    }
}