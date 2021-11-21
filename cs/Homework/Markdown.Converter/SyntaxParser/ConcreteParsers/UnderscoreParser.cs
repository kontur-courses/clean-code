using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.SyntaxParser.ConcreteParsers
{
    internal abstract class UnderscoreParser : ConcreteParser
    {
        private readonly Dictionary<TokenType, TokenType> oppositeTokenTypes = new()
        {
            {TokenType.Italics, TokenType.Bold},
            {TokenType.Bold, TokenType.Italics}
        };

        protected UnderscoreParser(ParseContext context) : base(context)
        {
        }

        protected TokenTree ParseUnderscore(Func<TokenTree> transformCurrentItem)
        {
            var current = Context.Current;
            if (ShouldParseAsText())
                return TokenTree.FromText(Context.Current.Value);
            var intersectionOffset = IsIntersectsWith(oppositeTokenTypes[current.TokenType]);
            if (intersectionOffset != -1)
                return ParseToText(intersectionOffset, Context.Current.Value);
            var buffer = new List<TokenTree>();
            do
            {
                Context.NextToken();
                if (Context.Current.TokenType == current.TokenType)
                    return new TokenTree(Context.Current, buffer.ToArray());
                buffer.Add(transformCurrentItem());
            } while (!Context.IsEndOfFileOrNewLine());

            return TokenTree.FromText(buffer.Aggregate(current.Value, (s, token) => s + token.Token.Value));
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
            if (ContainsWordsThatSeparatedBy(' '))
                return true;
            return false;
        }

        private bool SurroundedWithDigits() =>
            SurroundedWithDigits(Context.Previous, Context.LookAhead)
            || SurroundedWithDigits(Context.LookAhead, Context.Previous);

        private bool SurroundedWithDigits(Token before, Token after) =>
            char.IsDigit(before.Value.First()) && !char.IsWhiteSpace(after.Value.Last());

        private bool ContainsWordsThatSeparatedBy(char symbol)
        {
            var i = 1;
            Token currentToken;
            do
            {
                currentToken = Context.Peek(i);
                var value = currentToken.Value;
                if (value.Contains(symbol))
                    return true;
                i++;
            } while (!Context.IsEndOfFileOrNewLine() && currentToken.TokenType != Context.Current.TokenType);

            return false;
        }

        private TokenTree ParseToText(int endOffset, string prefix)
        {
            var buffer = new List<TokenTree>();
            for (var i = 0; i < endOffset; i++)
            {
                Context.NextToken();
                buffer.Add(TokenTree.FromText(Context.Current.Value));
            }

            return TokenTree.FromText(buffer.Aggregate(prefix, (s, token) => s + token.Token.Value));
        }

        private int IsIntersectsWith(TokenType tokenType)
        {
            var closingTag = GetOffsetOfFirstTagAppearanceInLine(Context.Current.TokenType);
            var otherOpeningTag = GetOffsetOfFirstTagAppearanceInLine(tokenType);
            if (otherOpeningTag == -1)
                return -1;
            var otherClosingTag = GetOffsetOfFirstTagAppearanceInLine(tokenType, otherOpeningTag + 1);
            var result = otherOpeningTag < closingTag && otherClosingTag > closingTag;
            return result ? otherClosingTag : -1;
        }

        private bool HasTokenInSameLine() => GetOffsetOfFirstTagAppearanceInLine(Context.Current.TokenType) != -1;

        private int GetOffsetOfFirstTagAppearanceInLine(TokenType tokenType, int startOffset = 1)
        {
            var i = startOffset;
            Token currentToken;
            do
            {
                currentToken = Context.Peek(i);
                if (currentToken.TokenType == tokenType && Context.CanBeClosedTag(i))
                    return i;
                i++;
            } while (currentToken.TokenType != TokenType.NewLine && Context.Position + i != Context.Tokens.Length);

            return -1;
        }
    }
}