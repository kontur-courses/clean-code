using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.SyntaxParser.ConcreteParsers
{
    internal abstract class UnderscoreParser : ConcreteParser
    {
        private readonly Func<TokenType, TokenType> oppositeTokenTypes =
            tokenType => tokenType switch
            {
                TokenType.Italics => TokenType.Bold,
                TokenType.Bold => TokenType.Italics,
                _ => throw new Exception($"unsupported token type: {tokenType}")
            };

        protected UnderscoreParser(ParseContext context) : base(context)
        {
        }

        protected TokenTree ParseUnderscore(Func<TokenTree> transformCurrentItem)
        {
            if (ShouldParseAsText())
                return TokenTree.FromText(Context.Current.Value);

            if (ParseHelper.TryGetIntersectionIndex(Context, oppositeTokenTypes(Context.Current.TokenType), out var intersectionOffset))
                return TokenTree.FromText(ParseToText(intersectionOffset, Context.Current.Value));

            return CreateToken(transformCurrentItem);
        }

        private TokenTree CreateToken(Func<TokenTree> transformCurrentItem)
        {
            var buffer = new List<TokenTree>();
            var current = Context.Current;
            do
            {
                Context.MoveToNextToken();
                if (Context.Current.TokenType == current.TokenType)
                    return new TokenTree(Context.Current, buffer.ToArray());
                buffer.Add(transformCurrentItem());
            } while (!Context.IsEndOfFileOrNewLine());

            var tokensText = buffer.Aggregate(current.Value, (s, tokenTree) => s + tokenTree.Token.Value);
            return TokenTree.FromText(tokensText);
        }

        private bool ShouldParseAsText()
        {
            if (Context.IsEndOfFileOrNewLine())
                return true;
            if (SurroundedWithDigits())
                return true;
            if (char.IsWhiteSpace(Context.LookAhead.Value.First()))
                return true;
            if (Context.LookAhead.TokenType == Context.Current.TokenType)
                return true;
            if (!HasTokenInSameLine())
                return true;
            if (ParseHelper.ContainsWordsThatSeparatedBy(Context, ' '))
                return true;
            return false;
        }

        private bool SurroundedWithDigits() =>
            SurroundedWithDigits(Context.Previous, Context.LookAhead)
            || SurroundedWithDigits(Context.LookAhead, Context.Previous);

        private bool SurroundedWithDigits(Token before, Token after) =>
            char.IsDigit(before.Value.First()) && !char.IsWhiteSpace(after.Value.Last());

        private bool HasTokenInSameLine() =>
            ParseHelper.TryGetOffsetOfFirstTagAppearanceInLine(Context, Context.Current.TokenType, out _);
    }
}