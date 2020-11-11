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
                [Styles.Bold] = Tuple.Create("<strong>", "</strong>")
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
            if (Value == Styles.Bold || Value == Styles.Italic)
                return !IsSpacesAlongBoldOrItalic(text) && !IsDigitsAlongBoldOrItalic(text);
            return false;
        }


        public bool IsBold(StringBuilder text)
        {
            if (EndPosition + 1 >= text.Length || text[EndPosition + 1].ToString() != Styles.Italic)
                return false;
            return text[EndPosition] == text[EndPosition + 1];
        }

        private bool IsSpacesAlongBoldOrItalic(StringBuilder text)
        {
            if (IsOpened)
                return EndPosition + 1 < text.Length && char.IsWhiteSpace(text[EndPosition + 1]);
            return char.IsWhiteSpace(text[StartPosition - 1]);
        }

        private bool IsDigitsAlongBoldOrItalic(StringBuilder text)
        {
            return StartPosition - 1 >= 0 && char.IsDigit(text[StartPosition - 1]) && EndPosition + 1 < text.Length
                   && char.IsDigit(text[EndPosition + 1]);
        }
    }
}