using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
    public static class StringUtils
    {
        public static bool IsWord(string text) => text.All(char.IsLetterOrDigit);
    }

    public class TokenParserIterator
    {
        private readonly BufferedEnumerator<Token> enumerator;
        private readonly Stack<TokenContext> contexts = new();
        private bool isWord;

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
                yield return context.Token.ToText().ToNode();
                foreach (var token in context.Children) yield return token;
            }
        }

        private TokenNode ParseToken(Token token)
        {
            return token.Type switch
            {
                TokenType.Escape => ParseEscapeCharacter(),
                TokenType.Cursive => ParseCursive(),
                TokenType.Bold => ParseBold(),
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
                if (context.Children.TrueForAll(x => x.Token.Type == TokenType.Text))
                {
                    var text = ConcatValues(context.Children.Select(x => x.Token));
                    return text.All(char.IsDigit) || isWord
                        ? Token.Text($"_{text}_").ToNode()
                        : new TokenNode(Token.Cursive, Token.Text(text).ToNode());
                }

                return new TokenNode(Token.Cursive, context.Children.ToArray());
            }

            if (enumerator.MoveNext())
            {
                contexts.Push(new TokenContext(Token.Cursive));
                return ParseToken(enumerator.Current);
            }

            return Token.Cursive.ToText().ToNode();
        }

        private TokenNode ParseBold()
        {
            if (contexts.TryPeek(out var peek))
            {
                if (peek.Token.Type == TokenType.Bold)
                {
                    var context = contexts.Pop();
                    if (context.Children.TrueForAll(x => x.Token.Type == TokenType.Text))
                    {
                        var text = ConcatValues(context.Children.Select(x => x.Token));
                        return text.All(char.IsDigit) || isWord
                            ? Token.Text($"__{text}__").ToNode()
                            : new TokenNode(Token.Bold, Token.Text(text).ToNode());
                    }

                    return new TokenNode(Token.Bold, context.Children.ToArray());
                }

                if (peek.Token.Type == TokenType.Cursive) return Token.Bold.ToText().ToNode();
            }

            if (enumerator.MoveNext())
            {
                contexts.Push(new TokenContext(Token.Bold));
                return ParseToken(enumerator.Current);
            }

            return Token.Bold.ToText().ToNode();
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
            var text = enumerator.Current.Value;
            sb.Append(text);
            isWord = !StringUtils.IsWord(text);
            while (enumerator.MoveNext())
            {
                var next = enumerator.Current;
                if (next.Type == TokenType.Text)
                {
                    isWord = !StringUtils.IsWord(next.Value);
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

        private static string ConcatValues(IEnumerable<Token> tokens) => string.Join("", tokens.Select(x => x.Value));
    }
}