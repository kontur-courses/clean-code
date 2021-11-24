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
            if (Iterator.TryPopContext(out var context))
                if (CanCloseContext(context, token))
                {
                    if (context.Children.TrueForAll(x => x.Token.Type == TokenType.Text))
                    {
                        var text = StringUtils.Join(context.Children.Select(x => x.Token));
                        return ShouldParseUnderscoreAsText(context, text)
                            ? Token.Text(StringUtils.Join(token, Token.Text(text), token)).ToNode()
                            : new TokenNode(token, Token.Text(text).ToNode());
                    }

                    return TryParseNonTextEntryOnSameTokenContext(context, out var node)
                        ? node
                        : new TokenNode(token, context.Children.ToArray());
                }
                else
                {
                    Iterator.PushContext(context);
                }


            var isInMiddleOfWord = IsInMiddleOfWord();
            if (CanOpenContext() && Iterator.TryMoveNext(out var next))
            {
                Iterator.PushContext(new TokenContext(token, isInMiddleOfWord));
                return Iterator.ParseToken(next);
            }

            return token.ToText().ToNode();
        }

        private bool CanOpenContext() => IsNextTokenStartsWithNonWhiteSpace();

        private bool CanCloseContext(TokenContext tokenContext, Token token)
        {
            var isAnyTokenInMiddleOfWord = tokenContext.IsInMiddleOfWord || IsInMiddleOfWord();
            var areBothTokensNotInMiddleOfWord = !isAnyTokenInMiddleOfWord;
            return tokenContext.Token.Type == token.Type
                   && IsPreviousTokenEndsWithNonWhiteSpace()
                   && (isAnyTokenInMiddleOfWord && !tokenContext.ContainsWhiteSpace || areBothTokensNotInMiddleOfWord);
        }

        private bool IsInMiddleOfWord() =>
            IsNextTokenStartsWithNonWhiteSpace() && IsPreviousTokenEndsWithNonWhiteSpace();

        
        private bool IsNextTokenStartsWithNonWhiteSpace() => Iterator.TryGetNextToken(out var token)
                                                             && token.Type == TokenType.Text
                                                             && token.Value.Length > 0
                                                             && char.IsLetterOrDigit(token.Value[0]);

        private bool IsPreviousTokenEndsWithNonWhiteSpace() => Iterator.TryGetPreviousToken(out var token)
                                                               && token.Type == TokenType.Text
                                                               && token.Value.Length > 0
                                                               && char.IsLetterOrDigit(token.Value[^1]);
        
        protected virtual bool TryParseNonTextEntryOnSameTokenContext(TokenContext _, out TokenNode token)
        {
            token = default;
            return false;
        }
        
        private static bool ShouldParseUnderscoreAsText(TokenContext context, string text)
        {
            return text.All(char.IsDigit) || context.IsInMiddleOfWord && text.Any(x => !char.IsLetter(x));
        }

    }
}