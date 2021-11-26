using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class BoldParsingIteratorState : UnderscoreParsingIteratorState
    {
        public BoldParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TagNode Parse() => ParseUnderscore(Token.Bold);

        protected override bool TryParseNonTextEntryOnSameTokenContext(TokenContext context, out TagNode tag)
        {
            tag = default;
            return false;
        }
    }
}