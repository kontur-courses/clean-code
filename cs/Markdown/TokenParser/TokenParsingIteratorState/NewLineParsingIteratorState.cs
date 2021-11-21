using System.Linq;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class NewLineParsingIteratorState : TokenParsingIteratorState
    {
        public NewLineParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TokenNode Parse()
        {
            if (Iterator.TryFlushContexts(out var node))
                return node.Token.Type == TokenType.Text
                    ? Token.Text(StringUtils.Join(node.Token, Token.NewLine)).ToNode()
                    : new TokenNode(node.Token, node.Children.Append(Token.NewLine.ToText().ToNode()).ToArray());
            return Token.NewLine.ToText().ToNode();
        }
    }
}