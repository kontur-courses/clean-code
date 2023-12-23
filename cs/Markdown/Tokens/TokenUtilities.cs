using Markdown.Tags;

namespace Markdown.Tokens
{
    public static class TokenUtilities
    {
        public static bool IsTextTokenEnd(char currentSymbol)
        {
            return IsEscapeTokenEnd(currentSymbol.ToString()) || IsTagStartingWithSymbol(currentSymbol);
        }

        public static bool IsTagTokenEnd(string previousSymbols)
        {
            return SupportedTags.Tags.TryGetValue(previousSymbols, out _);
        }

        public static bool IsEscapeTokenEnd(string previousSymbols)
        {
            return SupportedTags.Tags.TryGetValue(previousSymbols, out var tag) && tag.TagType == TagType.Escape;
        }

        public static bool IsTagStartingWithSymbol(char symbol)
        {
            return SupportedTags.Tags.Keys.Any(k => k.StartsWith(symbol));
        }

        public static TagType? GetTokenTagType(IToken? token)
        {
            if (token == null) return null;
            SupportedTags.Tags.TryGetValue(token.Content, out var tag);
            return tag?.TagType;
        }

        public static bool IsPreviousTokenCloseAndPrePreviousOpen(IToken? previous, IToken? prePrevious, string text)
        {
            return !SupportedTags.IsOpenTag(previous, text) && SupportedTags.IsOpenTag(prePrevious, text)
                    && previous != null && prePrevious != null
                    && SupportedTags.Tags.TryGetValue(previous.Content, out var previousTag)
                    && SupportedTags.Tags.TryGetValue(prePrevious.Content, out var prePreviousTag)
                    && previousTag.TagType == prePreviousTag.TagType;
        }

        public static (IToken Token, Tag? Tag) GetTokenAndTag(IToken token)
        {
            (IToken Token, Tag? Tag) tokenAndTag = (token, null);
            if (token.Type == TokenType.Tag)
            {
                SupportedTags.Tags.TryGetValue(token.Content, out var tag);
                tokenAndTag.Tag = tag;
            }
            else
            {
                tokenAndTag.Tag = null;
            }
            return tokenAndTag;
        }

        public static bool IsHaveDigitsBetweenTokens(IToken startToken, IToken endToken, string text)
        {
            return text[startToken.StartPosition..endToken.StartPosition].Any(char.IsDigit);
        }
    }
}
