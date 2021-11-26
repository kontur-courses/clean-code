using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public class EscapeParsingIteratorState : TokenParsingIteratorState
    {
        public EscapeParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        public override TagNode Parse()
        {
            if (Iterator.TryMoveNext(out var next))
                return next.Type switch
                {
                    TokenType.Cursive or TokenType.Escape => next.ToText().ToNode(),
                    TokenType.Bold => EscapeBold(),
                    TokenType.OpenSquareBracket => Token.OpenSquareBracket.ToText().ToNode(),
                    TokenType.CloseSquareBracket => EscapeSquareBracket(next),
                    _ => Token.Text(StringUtils.Join(Token.Escape, next)).ToNode()
                };

            return Token.Escape.ToText().ToNode();
        }

        private TagNode EscapeSquareBracket(Token token) => Iterator.AnyContext(TokenContext.IsLink)
            ? token.ToText().ToNode()
            : Tag.Text(StringUtils.Join(Token.Escape, token)).ToNode();

        private TagNode EscapeBold()
        {
            var cursive = Token.Cursive;
            Iterator.PushToBuffer(cursive);
            return cursive.ToText().ToNode();
        }
    }
}