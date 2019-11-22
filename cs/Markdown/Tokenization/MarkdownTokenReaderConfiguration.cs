using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Tokenization
{
    public class MarkdownTokenReaderConfiguration : ITokenReaderConfiguration
    {
        private static readonly HashSet<string> Separators = new HashSet<string> {"_", "\\_", "__", "\\__", "\\\\"};

        public bool IsSeparator(string text, int position)
        {
            CheckIfPositionIsCorrect(text, position);
            return Separators
                .Where(s => s.Length <= text.Length - position)
                .Any(s => text.Substring(position, s.Length) == s);
        }

        public int GetSeparatorLength(string text, int position)
        {
            CheckIfPositionIsCorrect(text, position);
            return Separators.OrderByDescending(s => s.Length)
                .Where(s => s.Length <= text.Length - position)
                .First(s => text.Substring(position, s.Length) == s)
                .Length;
        }

        public string GetSeparatorValue(string text, int position)
        {
            CheckIfPositionIsCorrect(text, position);
            return text.Substring(position, GetSeparatorLength(text, position));
        }

        private void CheckIfPositionIsCorrect(string text, int position)
        {
            if (position >= text.Length || position < 0)
                throw new ArgumentException($"position {position} is not in string with length {text.Length}");
        }
    }
}