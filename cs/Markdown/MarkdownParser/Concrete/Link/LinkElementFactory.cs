using System;
using System.Text;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Concrete.Special;
using MarkdownParser.Infrastructure.Markdown.Abstract;
using MarkdownParser.Infrastructure.Markdown.Models;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Concrete.Link
{
    public class LinkElementFactory : IMdElementFactory
    {
        public bool CanCreate(MarkdownElementContext context) => context.CurrentToken switch
        {
            SpecialToken special when special.Type == SpecialTokenType.OpeningRoundBracket =>
                CanCreateShortLink(context.NextTokens),
            SpecialToken special when special.Type == SpecialTokenType.OpeningSquareBracket =>
                CanCreateFullLink(context.NextTokens),
            _ => false
        };

        public MarkdownElement Create(MarkdownElementContext context) => context.CurrentToken switch
        {
            SpecialToken special when special.Type == SpecialTokenType.OpeningRoundBracket =>
                CreateShortLink(special, context.NextTokens),
            SpecialToken special when special.Type == SpecialTokenType.OpeningSquareBracket =>
                CreateFullLink(special, context.NextTokens),
            _ => throw new ArgumentException("Can't create link from specified context")
        };

        private static MarkdownElement CreateShortLink(SpecialToken opening, Token[] next)
        {
            return TryCollectTextUntil(next, SpecialTokenType.ClosingRoundBracket, 0, out var link, out var index)
                ? new MdElementLink(opening, new TextToken(opening.StartPosition, link), next[index])
                : throw new ArgumentException("Next tokens does not contain special token with type " +
                                              $"{SpecialTokenType.ClosingRoundBracket}");
        }

        private static MarkdownElement CreateFullLink(SpecialToken textOpening, Token[] next)
        {
            if (!TryCollectTextUntil(next, SpecialTokenType.ClosingSquareBracket, 0, out var text, out var closingPos))
                throw new ArgumentException("Next tokens does not contain special token with type " +
                                            $"{SpecialTokenType.ClosingSquareBracket}");

            var linkOpeningPos = closingPos + 1;
            if (next[linkOpeningPos] is SpecialToken special &&
                special.Type == SpecialTokenType.OpeningRoundBracket &&
                TryCollectTextUntil(next, SpecialTokenType.ClosingRoundBracket, linkOpeningPos + 1, out var link,
                    out var closingIndex))
            {
                return new MdElementLink(
                    textOpening, new TextToken(textOpening.StartPosition, text), next[closingPos],
                    next[linkOpeningPos], new TextToken(linkOpeningPos, link), next[closingIndex]);
            }

            throw new ArgumentException("Found closing square token, but can't find text closing token");
        }

        private static bool CanCreateFullLink(Token[] nextTokens)
        {
            var squareClosingIndex = GetTokenIndex(SpecialTokenType.ClosingSquareBracket, nextTokens);
            var roundOpeningIndex = GetTokenIndex(SpecialTokenType.OpeningRoundBracket, nextTokens);
            var roundClosingIndex = GetTokenIndex(SpecialTokenType.ClosingRoundBracket, nextTokens);
            return squareClosingIndex != -1 &&
                   roundOpeningIndex > squareClosingIndex &&
                   roundClosingIndex > roundOpeningIndex;
        }

        private static bool CanCreateShortLink(Token[] nextTokens) =>
            GetTokenIndex(SpecialTokenType.ClosingRoundBracket, nextTokens) != -1;

        private static int GetTokenIndex(SpecialTokenType tokenType, Token[] source)
        {
            for (var i = 0; i < source.Length; i++)
            {
                if (source[i] is SpecialToken specialToken && specialToken.Type == tokenType)
                    return i;
            }

            return -1;
        }

        private static bool TryCollectTextUntil(Token[] source, SpecialTokenType tokenType, int startIndex,
            out string collected, out int matchedIndex)
        {
            var builder = new StringBuilder(string.Empty);
            for (var i = startIndex; i < source.Length; i++)
            {
                var token = source[i];
                if (token is SpecialToken special && special.Type == tokenType)
                {
                    collected = builder.ToString();
                    matchedIndex = i;
                    return true;
                }

                builder.Append(token.RawValue);
            }

            matchedIndex = default;
            collected = default;
            return false;
        }
    }
}