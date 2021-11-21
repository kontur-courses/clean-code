using System.Linq;
using Markdown.Extensions;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public abstract class UnderscoreParsingIteratorState : TokenParsingIteratorState
    {
        protected UnderscoreParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        protected TokenNode ParseUnderscore(Token token)
        {
            if (Iterator.TryPeekContext(out var peek))
                if (peek.Token.Type == token.Type)
                {
                    var context = Iterator.PopContext();
                    if (context.Children.TrueForAll(x => x.Token.Type == TokenType.Text))
                    {
                        var text = StringUtils.Join(context.Children.Select(x => x.Token));
                        return ShouldParseUnderscoreAsText(context, text)
                            ? Token.Text(StringUtils.Join(token, Token.Text(text), token)).ToNode()
                            : new TokenNode(token, Token.Text(text).ToNode());
                    }

                    return TryParseEntryOnSameTokenContext(context, out var node)
                        ? node
                        : new TokenNode(token, context.Children.ToArray());
                }

            var isInMiddleOfWord = IsPreviousTokenContainsFirstPartOfWord() && IsNextTokenContainsSecondPartOfWord();
            if (Iterator.TryMoveNext(out var next))
            {
                Iterator.PushContext(new TokenContext(token, isInMiddleOfWord));
                return Iterator.ParseToken(next);
            }

            return token.ToText().ToNode();
        }

        private bool IsPreviousTokenContainsFirstPartOfWord() =>
            Iterator.TryGetPreviousToken(out var previous)
            && previous.Type == TokenType.Text
            && previous.Value.Length > 0
            && char.IsLetterOrDigit(previous.Value[^1]);

        private bool IsNextTokenContainsSecondPartOfWord() =>
            Iterator.TryGetNextToken(out var previous)
            && previous.Type == TokenType.Text
            && previous.Value.Length > 0
            && char.IsLetterOrDigit(previous.Value[0]);

        protected static bool ShouldParseUnderscoreAsText(TokenContext context, string text)
        {
            return text.All(char.IsDigit)
                   || text.StartsWith(" ")
                   || text.EndsWith(" ")
                   || context.IsSplitWord && text.Any(x => !char.IsLetter(x));
        }
        
        protected virtual bool TryParseEntryOnSameTokenContext(TokenContext _, out TokenNode token)
        {
            token = default;
            return false;
        }
    }
}