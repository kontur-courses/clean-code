using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class TextParsingIteratorState : TokenParsingIteratorState
    {
        public TextParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TagNode Parse() => Token.Text(Iterator.Current.Value).ToNode();
    }
}