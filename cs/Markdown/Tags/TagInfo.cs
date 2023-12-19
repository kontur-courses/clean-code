using Markdown.Tokens;

namespace Markdown.Tags
{
    public static class TagInfo
    {
        public readonly static Dictionary<string, Tag> SupportedTags = new()
        {
            { "_", new Tag("_", "em")},
            { "__", new Tag("__", "strong") },
            { "# ", new Tag("# ", "h1") },
            { "\n", new Tag("\n", "h1", false) },
            {"(", new Tag("(", "a") },
            {")", new Tag(")", "a", false) },
            {"[", new Tag("[", "a") },
            {"]", new Tag("]", "a", false) },
        };
        
        public static readonly HashSet<string> tagsSymbols = new()
        {
            "_", "__", "# ", "\n", "\\", "[", "]", "(", ")"
        };

        public readonly static string MarkdownStrongTag = "__";
        public readonly static string MarkdownItalicTag = "_";
        public readonly static string Escape = "\\";

        public static bool IsValidTokenTag(IToken token, string text)
        {
            SupportedTags.TryGetValue(token.Content, out var tag);
            if (tag == null)
                return false;
            return tag.TagType switch
            {
                TagType.Italic => IsStrongOrItalicTagCorrect(token.StartPosition, text),
                TagType.Header => IsHeaderTagCorrect(token.StartPosition, text, tag.IsOpen),
                TagType.Strong => IsStrongOrItalicTagCorrect(token.StartPosition, text),
                TagType.Link => IsLinkTagCorrect(token.StartPosition, text, tag.IsOpen),
                TagType.LinkDescription => IsLinkDescriptionTagCorrect(token.StartPosition, text, tag.IsOpen),
                _ => throw new ArgumentException($"tag of this type {tag.TagType} not supported")
            };
        }

        public static bool IsOpenTag(IToken? token, string text)
        {
            if (token == null) return false;
            SupportedTags.TryGetValue(token.Content, out var tag);
            if (tag == null) return false;
            return tag.TagType switch
            {
                TagType.Italic => IsStrongOrItalicOpenTag(token.StartPosition, text),
                TagType.Strong => IsStrongOrItalicOpenTag(token.StartPosition, text),
                TagType.Header => tag.IsOpen,
                TagType.Link => tag.IsOpen,
                TagType.LinkDescription => tag.IsOpen,
                _ => throw new ArgumentException($"tag of this type {tag.TagType} not supported")
            };
        }

        public static bool IsStrongOrItalicTagCorrect(int position, string text)
        {
            return IsStrongOrItalicCloseTag(position, text) ^ IsStrongOrItalicOpenTag(position, text);
        }

        public static bool IsHeaderTagCorrect(int position, string text, bool isOpen)
        {
            return !isOpen || position + 1 >= 0 && position + 1 < text.Length && char.IsWhiteSpace(text[position + 1]);
        }

        public static bool IsLinkTagCorrect(int position, string text, bool isOpen)
        {
            return !isOpen || IsLinkOpenTag(position, text);
        }

        public static bool IsLinkDescriptionTagCorrect(int position, string text, bool isOpen)
        {
            return isOpen || IsLinkDescriptionCloseTag(position, text);
        }

        public static bool IsStrongOrItalicOpenTag(int position, string text)
        {
            if (position - 1 >= 0 && position - 1 < text.Length
                && text[position - 1] == text[position])
                return IsStrongOrItalicOpenTag(position - 1, text);
            return (position == 0 || char.IsWhiteSpace(text[position - 1])
                    || tagsSymbols.Contains(text[position - 1].ToString()))
                    && position != text.Length - 1 
                    && !char.IsWhiteSpace(text[position + 1]);
        }

        public static bool IsStrongOrItalicCloseTag(int position, string text)
        {
            if (position + 1 >= 0 && position + 1 < text.Length 
                && text[position + 1] == text[position])
                return IsStrongOrItalicCloseTag(position + 1, text);
            return (position == text.Length - 1 || 
                    char.IsWhiteSpace(text[position + 1])
                    || tagsSymbols.Contains(text[position + 1].ToString()))
                    && position != 0 && !char.IsWhiteSpace(text[position - 1]);
        }

        public static bool IsLinkDescriptionCloseTag(int position, string text)
        {
            return position + 1 < text.Length && position + 1 > 1 
                   && SupportedTags.TryGetValue(text[position + 1].ToString(), out var tag)
                   && tag.TagType == TagType.Link
                   && tag.IsOpen;
        }

        public static bool IsLinkOpenTag(int position, string text)
        {
            return position - 1 < text.Length - 1 && position - 1 > 0 
                   && SupportedTags.TryGetValue(text[position - 1].ToString(), out var tag)
                   && tag.TagType == TagType.LinkDescription
                   && !tag.IsOpen;
        }
    }
}
