using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownTag
    {
        public static readonly Dictionary<string, Tuple<string, string>> MatchingMarkdownTagsToHtmlTags =
            new Dictionary<string, Tuple<string, string>>
            {
                ["_"] = Tuple.Create("<em>", "</em>"),
                ["__"] = Tuple.Create("<strong>", "</strong>")
            };

        public MarkdownTag(string value, int index, bool isOpened)
        {
            Value = value;
            Index = index;
            IsOpened = isOpened;
        }

        public string Value { get; }
        public int Index { get; }
        public bool IsOpened { get; }

        public static bool IsTag(string symbol)
        {
            return MatchingMarkdownTagsToHtmlTags.ContainsKey(symbol);
        }

        public bool IsValidTag(string text, int index)
        {
            if (Value != "_")
                return true;
            if (IsSpacesAlongUnderline(text, index))
                return false;
            if (index + 1 < text.Length && index - 1 >= 0 && char.IsDigit(text[index - 1])
                && char.IsDigit(text[index + 1]))
                return false;
            return true;
        }

        private bool IsSpacesAlongUnderline(string text, int index)
        {
            if (IsOpened)
                return index + 1 < text.Length && text[index + 1] == ' ';
            return text[index - 1] == ' ';
        }
    }
}