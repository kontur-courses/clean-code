using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParser
{
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

            if (TryFlushContexts(out var contextNode)) yield return contextNode;
        }

        private bool TryFlushContexts(out TokenNode node)
        {
            node = default;
            if (contexts.Count > 0)
            {
                var stack = new Stack<string>();
                while (contexts.TryPop(out var context))
                    if (context.Token.Type != TokenType.Header1)
                    {
                        stack.Push(context.ToText());
                    }
                    else
                    {
                        var text = string.Join("", stack);
                        context.Children.Add(Token.Text(text).ToNode());
                        node = new TokenNode(Token.Header1, context.Children.ToArray());
                        contexts.Clear();
                        return true;
                    }

                node = Token.Text(string.Join("", stack)).ToNode();
                contexts.Clear();
                return true;
            }

            return false;
        }

        private TokenNode ParseToken(Token token)
        {
            return token.Type switch
            {
                TokenType.Escape => ParseEscapeCharacter(),
                TokenType.Cursive => ParseCursive(),
                TokenType.Bold => ParseBold(),
                TokenType.Header1 => ParseHeader1(),
                TokenType.NewLine => ParseNewLine(),
                _ => ParseTextToken()
            };
        }

        private TokenNode ParseNewLine()
        {
            if (TryFlushContexts(out var node))
            {
                if (node.Token.Type == TokenType.Text)
                    return Token.Text($"{node.Token.Value}{Token.NewLine.Value}").ToNode();
                return new TokenNode(node.Token, node.Children.Append(Token.NewLine.ToText().ToNode()).ToArray());
            }

            return Token.NewLine.ToText().ToNode();
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

        private TokenNode ParseHeader1()
        {
            if (enumerator.MoveNext())
            {
                contexts.Push(new TokenContext(Token.Header1, false));
                return ParseToken(enumerator.Current);
            }

            return Token.Header1.ToNode();
        }

        private TokenNode ParseCursive() => ParseUnderscore(Token.Cursive);

        private TokenNode ParseBold() => ParseUnderscore(Token.Bold);

        private TokenNode ParseUnderscore(Token token)
        {
            if (contexts.TryPeek(out var peek))
                if (peek.Token.Type == token.Type)
                {
                    var context = contexts.Pop();
                    if (context.Children.TrueForAll(x => x.Token.Type == TokenType.Text))
                    {
                        var text = ConcatValues(context.Children.Select(x => x.Token));
                        return ShouldParseUnderscoreAsText(context, text)
                            ? Token.Text(ConcatValues(token, Token.Text(text), token)).ToNode()
                            : new TokenNode(token, Token.Text(text).ToNode());
                    }

                    if (peek.Token.Type == TokenType.Cursive)
                    {
                        var text = string.Join("", context.Children.Select(x => x.ToText()));
                        return ShouldParseUnderscoreAsText(context, text)
                            ? Token.Text(ConcatValues(token, Token.Text(text), token)).ToNode()
                            : new TokenNode(token, Token.Text(text).ToNode());
                    }

                    return new TokenNode(token, context.Children.ToArray());
                }

            var isInMiddleOfWord = IsPreviousTokenContainsFirstPartOfWord() && IsNextTokenContainsSecondPartOfWord();
            if (enumerator.MoveNext())
            {
                contexts.Push(new TokenContext(token, isInMiddleOfWord));
                return ParseToken(enumerator.Current);
            }

            return token.ToText().ToNode();
        }

        private bool IsPreviousTokenContainsFirstPartOfWord() =>
            enumerator.TryGetPrevious(out var previous)
            && previous.Type == TokenType.Text
            && previous.Value.Length > 0
            && char.IsLetterOrDigit(previous.Value[^1]);

        private bool IsNextTokenContainsSecondPartOfWord() =>
            enumerator.TryGetNext(out var previous)
            && previous.Type == TokenType.Text
            && previous.Value.Length > 0
            && char.IsLetterOrDigit(previous.Value[0]);

        private static bool ShouldParseUnderscoreAsText(TokenContext context, string text)
        {
            return text.All(char.IsDigit)
                   || text.StartsWith(" ")
                   || text.EndsWith(" ")
                   || context.IsSplitWord && text.Any(x => !char.IsLetter(x));
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

        private static string ConcatValues(IEnumerable<Token> tokens) => string.Join("", tokens.Select(x => x.Value));

        private static string ConcatValues(params Token[] tokens) => ConcatValues((IEnumerable<Token>)tokens);
    }
}