using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags.TagAndTokenComparers.PairTagAndTokenComparers
{
    class DefaultPairTagAndTokenComparer : IPairTagAndTokenComparer
    {
        private readonly char ignorableSymbol;

        public DefaultPairTagAndTokenComparer() => ignorableSymbol = ' ';

        public bool CloseTagIfNotFoundClosingTag => false;

        public bool IsTokenOpenTag(Token token, Tag openTag) =>
            !IgnoreTag(token, openTag, token.StartIndex + token.Length);

        public bool IsTokenCloseTag(Token token, Tag closeTag) =>
            !IgnoreTag(token, closeTag, token.StartIndex - 1);

        private bool IgnoreTag(Token token, Tag tag, int ignorableSymbolIndex)
        {
            var containsIgnorableSymbol =
                ignorableSymbolIndex >= 0 &&
                ignorableSymbolIndex < token.Str.Length &&
                token.Str[ignorableSymbolIndex] == ignorableSymbol;
            return
                containsIgnorableSymbol ||
                token.Str.Substring(token.StartIndex, token.Length) != tag.Value ||
                IsTokenNearWithDigit(token);
        }

        private bool IsTokenNearWithDigit(Token token)
        {
            var leftIndex = token.StartIndex - 1;
            var rightIndex = token.StartIndex + token.Length;
            return
                (leftIndex >= 0 && char.IsDigit(token.Str[leftIndex])) ||
                (rightIndex < token.Str.Length && char.IsDigit(token.Str[rightIndex]));
        }
    }
}