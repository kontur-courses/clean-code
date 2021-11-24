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
            {
                if (node.Token.Type == TokenType.Text)
                    return Token.Text(StringUtils.Join(node.Token, Token.NewLine)).ToNode();

                Iterator.PushToBuffer(Token.NewLine);
                return new TokenNode(node.Token, node.Children);
            }

            return Token.NewLine.ToText().ToNode();
        }
    }
}