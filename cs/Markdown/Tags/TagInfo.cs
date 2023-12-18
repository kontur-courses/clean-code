using Markdown.Tokens;

namespace Markdown.Tags
{
    public static class TagInfo
    {
        public static Tag? GetTagByMarkdownValue(string? value)
        {
            return value switch
            {
                "_" => new Tag("_", "em"),
                "__" => new Tag("__", "strong"),
                "# " => new Tag("# ", "h1"),
                "\n" => new Tag("\n", "h1", false),
                _ => null
            };
        }

        public readonly static string MarkdownStrongTag = "__";
        public readonly static string MarkdownItalicTag = "_";
        public readonly static string Escape = "\\";

        public static bool IsValidTokenTag(IToken token, string text)
        {
            var tag = GetTagByMarkdownValue(token.Content);
            if (tag == null)
                return false;
            return tag.TagType switch
            {
                TagType.Italic => IsStrongOrItalicTagCorrect(token.StartPosition, token.Content, text),
                TagType.Header => IsHeaderTagCorrect(token.StartPosition, text, tag.IsOpen),
                TagType.Strong => IsStrongOrItalicCloseTag(token.StartPosition, text),
                _ => throw new ArgumentException($"tag of this type {tag.TagType} not supported")
            };
        }

        public static bool IsOpenTag(IToken? token, string text)
        {
            var tag = GetTagByMarkdownValue(token?.Content);
            if (tag == null) return false;
            return tag.TagType switch
            {
                TagType.Italic => IsStrongOrItalicOpenTag(token.StartPosition, text),
                TagType.Strong => IsStrongOrItalicOpenTag(token.StartPosition, text),
                TagType.Header => tag.IsOpen,
                _ => throw new ArgumentException($"tag of this type {tag.TagType} not supported")
            };
        }
        public static bool IsStrongOrItalicTagCorrect(int position, string content, string text)
        {
            return IsStrongOrItalicCloseTag(position, text) ^ IsStrongOrItalicOpenTag(position, text);
        }

        public static bool IsHeaderTagCorrect(int position, string text, bool isOpen)
        {
            return isOpen switch
            {
                true => position + 1 >= 0 && position + 1 < text.Length && char.IsWhiteSpace(text[position + 1]),
                false => true
            };
        }

        public static bool IsStrongOrItalicOpenTag(int position, string text)
        {
            if (position - 1 >= 0 && position - 1 < text.Length
                && text[position - 1] == text[position])
                return IsStrongOrItalicOpenTag(position - 1, text);
            return (position == 0 || char.IsWhiteSpace(text[position - 1]))
                    && position != text.Length - 1 
                    && !char.IsWhiteSpace(text[position + 1]);
        }

        public static bool IsStrongOrItalicCloseTag(int position, string text)
        {
            if (position + 1 >= 0 && position + 1 < text.Length 
                && text[position + 1] == text[position])
                return IsStrongOrItalicCloseTag(position + 1, text);
            return (position == text.Length - 1 || 
                    char.IsWhiteSpace(text[position + 1]))
                    && position != 0 && !char.IsWhiteSpace(text[position - 1]);
        }
    }
}
