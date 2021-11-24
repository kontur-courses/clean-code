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

        protected override bool TryParseNonTextEntryOnSameTokenContext(TokenContext context, out TokenNode node)
        {
            var token = Token.Cursive;
            var children = context.Children
                .Select(x => x.Token.Type == TokenType.Bold ? Token.Text(x.ToText()).ToNode() : x)
                .ToArray();
            node = new TokenNode(token, children);
            return true;
        }
    }
}