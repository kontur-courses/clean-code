using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags.TagAndTokenComparers.PairTagAndTokenComparers
{
    class BlockquotesAndTokenComparer : IPairTagAndTokenComparer
    {
        public bool CloseTagIfNotFoundClosingTag => true;

        public bool IsTokenCloseTag(Token token, Tag closeTag) => IsTokenTag(token, closeTag);
        public bool IsTokenOpenTag(Token token, Tag openTag) => IsTokenTag(token, openTag);
        private bool IsTokenTag(Token token, Tag tag) =>
            token.Str.Substring(token.StartIndex, token.Length) == tag.Value;
    }
}