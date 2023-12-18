using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parsers.Markdown.Rules.TagsRules
{
    public static class ItalicTagRules
    {
        public static bool IsItalicTagIgnoredBySymbol(char symbol)
        {
            return char.IsDigit(symbol);
        }

        public static bool IsItalicTagOpen(char previousSymbol, char nextSymbol)
        {
            return char.IsWhiteSpace(previousSymbol) && !char.IsWhiteSpace(nextSymbol) || previousSymbol == MarkdownRules.EmptyChar;
        }

        public static bool IsItalicTagClosing(char previousSymbol, char nextSymbol)
        {
            return char.IsWhiteSpace(nextSymbol) && !char.IsWhiteSpace(previousSymbol) || nextSymbol == MarkdownRules.EmptyChar;
        }

        public static bool IsItalicTagsPaired(TagToken firstTag, TagToken secondTag, Dictionary<int, TagToken> parsedTokens)
        {
            if ((firstTag.Value.TagType == TagType.Undefined || secondTag.Value.TagType == TagType.Undefined)
                && MarkdownRules.IsTagsInsideOneWord(firstTag, secondTag, parsedTokens))
                return true;
            return firstTag.Value.TagType == TagType.OpenTag && secondTag.Value.TagType == TagType.ClosingTag;
        }

        public static bool IsItalicTagsIgnored(TagToken firstTag, TagToken secondTag)
        {
            return firstTag.EndIndex + 1 == secondTag.StartIndex;
        }
    }
}
