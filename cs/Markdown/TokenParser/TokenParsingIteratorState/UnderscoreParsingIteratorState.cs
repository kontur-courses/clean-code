using System.Linq;
using Markdown.Extensions;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.TokenParser.TokenParsingIteratorState
{
    public abstract class UnderscoreParsingIteratorState : TokenParsingIteratorState
    {
        protected UnderscoreParsingIteratorState(TokenParsingIterator iterator) : base(iterator)
        {
        }

        protected TagNode ParseUnderscore(Token token)
        {
            if (Iterator.TryPopContext(out var context))
                if (CanCloseContext(context, token))
                {
                    if (context.Children.All(n => n.Tag.Type == TagType.Text))
                    {
                        var textParts = context.Children
                            .Select(n => ConvertToToken(n.Tag).GetText());
                        var text = StringUtils.Join(textParts);
                        return ShouldParseUnderscoreAsText(context, text)
                            ? Token.Text(StringUtils.Join(token, Token.Text(text), token)).ToNode()
                            : new TagNode(token.ToTag(), Token.Text(text).ToNode());
                    }

                    return TryParseNonTextEntryOnSameTokenContext(context, out var node)
                        ? node
                        : new TagNode(token.ToTag(), context.Children.ToArray());
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

        protected abstract bool TryParseNonTextEntryOnSameTokenContext(TokenContext context, out TagNode tag);

        private static Token ConvertToToken(Tag tag)
        {
            return tag.Type switch
            {
                TagType.Bold => Token.Bold,
                TagType.Cursive => Token.Cursive,
                _ => Token.Text(tag.GetText())
            };
        }

        private static bool ShouldParseUnderscoreAsText(TokenContext context, string text)
        {
            return text.All(char.IsDigit) || context.IsInMiddleOfWord && text.Any(x => !char.IsLetter(x));
        }
    }
}