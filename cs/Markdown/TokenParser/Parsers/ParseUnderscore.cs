using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class ParseUnderscore : Parser
    { 
        public ParseUnderscore(IEnumerable<IToken> tokens) : base(tokens) { }
        
        public override TokenTree ParseToken(int position)
        {
            var token = Tokens[position];
            if (MustParseAsText(token.TokenType, position))
                return new ParseAsText(Tokens).ParseToken(position);
            if (HasIntersectionWithOppositeTokenType(position, GetOppositeTokenType(token.TokenType), out var intersectedIndex))
                return new ParseAsText(Tokens).ParseTokens(position, intersectedIndex);
            var tree = new TokenTree(token);
            position++;
            while (position < Tokens.Count && Tokens[position].TokenType != token.TokenType)
            {
                var component = base.ParseToken(position);
                tree.Add(component);
                position += component.Count;
            }
            return tree;
        }
        
        private TokenType GetOppositeTokenType(TokenType tokenType)
        {
            return tokenType switch
            {
                TokenType.Italics => TokenType.Strong,
                TokenType.Strong => TokenType.Italics,
                _ => throw new ArgumentException($"Unsupported tokenType: {tokenType}")
            };
        }
        
        private bool MustParseAsText(TokenType tokenType, int position)
        {
            if (position + 1 == Tokens.Count)
                return true;
            if (string.IsNullOrEmpty(NextToken(position).Value))
                return true;
            if (IsNumbersNearby(position))
                return true;
            if (!HasCloseTokenInLine(tokenType, position + 1))
                return true;
            if (NextSymbolsInLineContainsWhiteSpace(tokenType, position + 1))
                return true;
            return false;
        }

        private bool NextSymbolsInLineContainsWhiteSpace(TokenType tokenType, int position)
        {
            for (var i = position; i < Tokens.Count; i++)
            {
                var token = Tokens[i];
                if (token.Value.Contains(' '))
                    return true;
                if (token.TokenType == tokenType || token.TokenType == TokenType.NewLine)
                    return false;
            }

            return false;
        }

        private bool IsNumbersNearby(int position)
        {
            var previous = PreviousToken(position);
            var next = NextToken(position);
            return SurroundedWithDigitAndWhitespace(previous, next) ||
                   SurroundedWithDigitAndWhitespace(next, previous);
        }

        private bool SurroundedWithDigitAndWhitespace(IToken first, IToken second) 
            => char.IsDigit(first.Value.First()) && !char.IsWhiteSpace(second.Value.Last());

        private bool HasIntersectionWithOppositeTokenType(int position, TokenType tokenType, out int oppositeCloseIndex)
        {
            var token = Tokens[position];
            oppositeCloseIndex = 0;
            if (!TryGetFirstIndexOfTokenInLine(token.TokenType, position, out var closeIndex))
                return false;
            if (!TryGetFirstIndexOfTokenInLine(tokenType, position, out var oppositeOpenIndex))
                return false;
            if (!TryGetFirstIndexOfTokenInLine(tokenType, position, out oppositeCloseIndex, oppositeOpenIndex + 1))
                return false;

            return oppositeOpenIndex < closeIndex && oppositeCloseIndex > closeIndex;
        }
    }
}