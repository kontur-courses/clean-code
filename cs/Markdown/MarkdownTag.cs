using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class MarkdownTag
    {
        public static readonly Dictionary<string, Tuple<string, string>> MatchingMarkdownTagsToHtmlTags =
            new Dictionary<string, Tuple<string, string>>
            {
                [Styles.Italic] = Tuple.Create("<em>", "</em>"),
                [Styles.Bold] = Tuple.Create("<strong>", "</strong>"),
                [Styles.Heading] = Tuple.Create("<h1>", "</h1>")
            };

        public MarkdownTag(string value, int startPosition, bool isOpened)
        {
            Value = value;
            StartPosition = startPosition;
            EndPosition = startPosition + value.Length - 1;
            IsOpened = isOpened;
        }

        public string Value { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public bool IsOpened { get; }
        public int Length => EndPosition - StartPosition + 1;

        public static bool IsTag(string symbol)
        {
            return MatchingMarkdownTagsToHtmlTags.ContainsKey(symbol);
        }

        public bool IsValidTag(StringBuilder text)
        {
            if (Value == Styles.Heading)
                return StartPosition == 0 && IsSpacesAlongTag(text);
            if (Value == Styles.Bold || Value == Styles.Italic)
                return !IsSpacesAlongTag(text) && !IsDigitsAlongTag(text);
            return true;
        }

        public bool IsShieldedTag(Stack<MarkdownTag> tags)
        {
            return tags.Count > 0 && tags.Peek().Value == @"\";
        }

        public bool IsBold(StringBuilder text)
        {
            if (EndPosition + 1 >= text.Length || text[EndPosition + 1].ToString() != Styles.Italic)
                return false;
            return text[EndPosition] == text[EndPosition + 1];
        }

        private bool IsSpacesAlongTag(StringBuilder text)
        {
            if (IsOpened)
                return EndPosition + 1 < text.Length && char.IsWhiteSpace(text[EndPosition + 1]);
            return char.IsWhiteSpace(text[StartPosition - 1]);
        }

        private bool IsDigitsAlongTag(StringBuilder text)
        {
            return StartPosition - 1 >= 0 && char.IsDigit(text[StartPosition - 1]) && EndPosition + 1 < text.Length
                   && char.IsDigit(text[EndPosition + 1]);
        }
    }
}