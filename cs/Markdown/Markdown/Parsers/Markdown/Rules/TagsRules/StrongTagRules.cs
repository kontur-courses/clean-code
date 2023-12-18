using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parsers.Markdown.Rules.TagsRules
{
    public static class StrongTagRules
    {
        public static bool IsStrongTagIgnoredBySymbol(char symbol)
        {
            return char.IsDigit(symbol);
        }

        public static bool IsStrongTagOpen(char previousSymbol, char nextSymbol)
        {
            return char.IsWhiteSpace(previousSymbol) && !char.IsWhiteSpace(nextSymbol) || previousSymbol == MarkdownRules.EmptyChar;
        }

        public static bool IsStrongTagClosing(char previousSymbol, char nextSymbol)
        {
            return char.IsWhiteSpace(nextSymbol) && !char.IsWhiteSpace(previousSymbol) || nextSymbol == MarkdownRules.EmptyChar;
        }

        public static bool IsStrongTagsPaired(TagToken firstTag, TagToken secondTag, Dictionary<int, TagToken> parsedTokens)
        {
            if ((firstTag.Value.TagType == TagType.Undefined || secondTag.Value.TagType == TagType.Undefined) 
                && MarkdownRules.IsTagsInsideOneWord(firstTag, secondTag, parsedTokens))
                return true;
            return firstTag.Value.TagType == TagType.OpenTag && secondTag.Value.TagType == TagType.ClosingTag;
        }

        public static bool IsStrongTagsIgnored(TagToken firstTag, TagToken secondTag)
        {
            return firstTag.EndIndex + 1 == secondTag.StartIndex;
        }
    }
}
