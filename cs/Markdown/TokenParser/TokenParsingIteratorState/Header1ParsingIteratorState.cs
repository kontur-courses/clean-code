using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class Header1ParsingIteratorState : TokenParsingIteratorState
    {
        public Header1ParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TagNode Parse()
        {
            if (Iterator.TryMoveNext(out var next))
            {
                Iterator.PushContext(new TokenContext(Token.Header1, false));
                return Iterator.ParseToken(next);
            }

            return Token.Header1.ToNode();
        }
    }
}