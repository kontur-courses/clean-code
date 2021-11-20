using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TokenContext
    {
        public Token Token { get; }
        public List<TokenNode> Children { get; } = new();

        public TokenContext(Token token)
        {
            Token = token;
        }
    }

    public class TokenParserIterator
    {
        private readonly BufferedEnumerator<Token> enumerator;

        private readonly Stack<TokenContext> contexts = new();

        public TokenParserIterator(IEnumerator<Token> tokens)
        {
            enumerator = new BufferedEnumerator<Token>(tokens);
        }

        public IEnumerable<TokenNode> Parse()
        {
            while (enumerator.MoveNext())
            {
                var node = ParseToken(enumerator.Current);
                if (contexts.TryPeek(out var context))
                    context.Children.Add(node);
                else
                    yield return node;
            }

            while (contexts.TryPop(out var context))
            {
                yield return Token.Text(context.Token.Value).ToNode();
                foreach (var token in context.Children) yield return token;
            }
        }

        private TokenNode ParseToken(Token token)
        {
            return token.Type switch
            {
                TokenType.Escape => ParseEscapeCharacter(),
                TokenType.Cursive => ParseCursive(),
                _ => ParseTextToken()
            };
        }

        private TokenNode ParseEscapeCharacter()
        {
            var current = enumerator.Current;
            if (enumerator.MoveNext())
            {
                var next = enumerator.Current;
                return next.Type switch
                {
                    TokenType.Cursive or TokenType.Escape => next.ToText().ToNode(),
                    TokenType.Bold => EscapeBold(),
                    _ => Token.Text(Token.Escape.Value + next.Value).ToNode()
                };
            }

            return current.ToText().ToNode();
        }

        private TokenNode ParseCursive()
        {
            if (contexts.TryPeek(out var peek) && peek.Token.Type == TokenType.Cursive)
            {
                var context = contexts.Pop();
                return new TokenNode(Token.Cursive, context.Children.ToArray());
            }

            if (enumerator.MoveNext())
            {
                contexts.Push(new TokenContext(Token.Cursive));
                return ParseToken(enumerator.Current);
            }

            return Token.Text(Token.Cursive.Value).ToNode();
        }

        private TokenNode EscapeBold()
        {
            var cursive = Token.Cursive;
            enumerator.PushToBuffer(cursive);
            return cursive.ToText().ToNode();
        }

        private TokenNode ParseTextToken()
        {
            var sb = new StringBuilder();
            sb.Append(enumerator.Current.Value);
            while (enumerator.MoveNext())
            {
                var next = enumerator.Current;
                if (next.Type == TokenType.Text)
                {
                    sb.Append(next.Value);
                }
                else
                {
                    enumerator.PushToBuffer(next);
                    break;
                }
            }

            return Token.Text(sb.ToString()).ToNode();
        }
    }
}