using System.Linq;
using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class CursiveParsingIteratorState : UnderscoreParsingIteratorState
    {
        public CursiveParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TagNode Parse() => ParseUnderscore(Token.Cursive);

        protected override bool TryParseNonTextEntryOnSameTokenContext(TokenContext context, out TagNode node)
        {
            var token = Token.Cursive;
            var children = context.Children
                .Select(x => x.Tag.Type == TagType.Bold ? Tag.Text(x.ToText()).ToNode() : x)
                .ToArray();
            node = new TagNode(token.ToTag(), children);
            return true;
        }
    }
}