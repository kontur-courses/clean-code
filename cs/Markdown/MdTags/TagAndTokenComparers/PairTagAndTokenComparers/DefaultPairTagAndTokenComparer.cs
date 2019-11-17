using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags.TagAndTokenComparers.PairTagAndTokenComparers
{
    class DefaultPairTagAndTokenComparer : IPairTagAndTokenComparer
    {
        private readonly char ignorableSymbol;

        public DefaultPairTagAndTokenComparer() => ignorableSymbol = ' ';

        public bool IsTokenOpenTag(Token token, Tag openTag) =>
            !IgnoreTag(token, openTag, token.StartIndex + token.Length);

        public bool IsTokenCloseTag(Token token, Tag closeTag) =>
            !IgnoreTag(token, closeTag, token.StartIndex - 1);

        private bool IgnoreTag(Token token, Tag tag, int ignorableSymbolIndex)
        {
            if (token.Str.Substring(token.StartIndex, token.Length) != tag.Value ||
                (
                    ignorableSymbolIndex >= 0 &&
                    ignorableSymbolIndex < token.Str.Length &&
                    token.Str[ignorableSymbolIndex] == ignorableSymbol
                ))
                return true;
            return IsTagInsideDigits(token);
        }

        private bool IsTagInsideDigits(Token token)
        {
            var leftIndex = token.StartIndex - 1;
            var rightIndex = token.StartIndex + token.Length;
            if (leftIndex < 0 || rightIndex >= token.Str.Length)
                return false;
            return char.IsDigit(token.Str[leftIndex]) && char.IsDigit(token.Str[rightIndex]);
        }
    }
}