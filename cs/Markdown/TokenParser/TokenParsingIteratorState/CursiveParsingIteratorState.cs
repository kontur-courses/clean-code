using System.Linq;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class CursiveParsingIteratorState : UnderscoreParsingIteratorState
    {
        public CursiveParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TokenNode Parse() => ParseUnderscore(Token.Cursive);

        protected override bool TryParseEntryOnSameTokenContext(TokenContext context, out TokenNode node)
        {
            var token = Token.Cursive;
            var text = StringUtils.Join(context.Children.Select(x => x.ToText()));
            node = ShouldParseUnderscoreAsText(context, text)
                ? Token.Text(StringUtils.Join(token, Token.Text(text), token)).ToNode()
                : new TokenNode(token, Token.Text(text).ToNode());
            return true;
        }
    }
}