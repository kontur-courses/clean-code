using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class EscapeParsingIteratorState : TokenParsingIteratorState
    {
        public EscapeParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TokenNode Parse()
        {
            if (Iterator.TryMoveNext(out var next))
                return next.Type switch
                {
                    TokenType.Cursive or TokenType.Escape => next.ToText().ToNode(),
                    TokenType.Bold => EscapeBold(),
                    _ => Token.Text(StringUtils.Join(Token.Escape, next)).ToNode()
                };

            return Token.Escape.ToText().ToNode();
        }

        private TokenNode EscapeBold()
        {
            var cursive = Token.Cursive;
            Iterator.PushToBuffer(cursive);
            return cursive.ToText().ToNode();
        }
    }
}