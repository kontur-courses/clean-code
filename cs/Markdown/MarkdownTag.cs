using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class MarkdownTag
    {
        public static readonly Dictionary<string, (string, string)> MatchingMarkdownTagsToHtmlTags =
            new Dictionary<string, (string, string)>
            {
                [TokenType.Italic] = ("<em>", "</em>"),
                [TokenType.Bold] = ("<strong>", "</strong>"),
                [TokenType.Heading] = ("<h1>", "</h1>")
            };

        public MarkdownTag(string value, int position, bool isOpened)
        {
            Value = value;
            StartPosition = position;
            EndPosition = StartPosition + Value.Length - 1;
            IsOpened = isOpened;
        }

        public string Value { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }
        public bool IsOpened { get; }
        public int Length => EndPosition - StartPosition + 1;

        public static string CreateHtmlLink(string link, string title)
            => $"<a href=\"{link}\">{title}</a>";

        public bool IsValidTag(StringBuilder text)
        {
            if (Value == TokenType.Heading)
                return StartPosition == 0 && IsSpacesAlongTag(text);
            if (Value == TokenType.Bold || Value == TokenType.Italic)
                return !IsSpacesAlongTag(text) && !IsDigitsAlongTag(text);
            return true;
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