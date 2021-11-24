using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public abstract class TokenParsingIteratorState
    {
        protected readonly TokenParsingIterator Iterator;

        protected TokenParsingIteratorState(TokenParsingIterator iterator)
        {
            Iterator = iterator;
        }

        public abstract TagNode Parse();
    }
}