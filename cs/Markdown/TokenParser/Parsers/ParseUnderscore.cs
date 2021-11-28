using System;
using System.Collections.Generic;
using System.Linq;
using Markdow.Interfaces;

namespace Markdown.TokenParser.Parsers
{
    public class ParseUnderscore : Parser, IConcreteParser
    { 
        public ParseUnderscore(IEnumerable<IToken> tokens) : base(tokens) { }
        
        public override TokenTree ParseToken(int position)
        {
            var token = Tokens[position];
            if (MustParseAsText(token.TokenType, position))
                return new ParseAsText(Tokens).ParseToken(position);
            if (HasIntersectionWithOppositeTokenType(position, GetOppositeTokenType(token.TokenType), out var intersectedIndex))
                return new ParseAsText(Tokens).ParseTokens(position, intersectedIndex);
            if (!IsStrongAndItalicsSequenceRight(GetOppositeTokenType(token.TokenType), position, out var closeIndex))
                return new ParseAsText(Tokens).ParseTokens(position, closeIndex);
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
            if (NextToken(position).TokenType == TokenType.WhiteSpace)
                return true;
            if (IsNumbersNearby(position))
                return true;
            if (!HasCloseTokenInLine(tokenType, position + 1))
                return true;
            TryGetFirstIndexOfTokenInLine(Tokens[position].TokenType, position, out var closeIndex);
            if (!CheckUnderscoreWithWhiteSpacesRules(position))
                return true;
            return false;
        }

        private bool CheckUnderscoreWithWhiteSpacesRules(int position)
        {
            TryGetFirstIndexOfTokenInLine(Tokens[position].TokenType, position, out var closeIndex);
            if (StartsAndEndWithWhiteSpace(position, closeIndex))
                return true;
            return !IsWhiteSpaceBetweenOpenAndClose(position, closeIndex);
        }

        private bool StartsAndEndWithWhiteSpace(int position, int closeIndex) =>
            PreviousToken(position).TokenType == TokenType.WhiteSpace &&
            NextToken(closeIndex).TokenType == TokenType.WhiteSpace;

        private bool IsWhiteSpaceBetweenOpenAndClose(int position, int closeIndex)
        {
            for (var i = position; i < closeIndex; i++)
            {
                if (NextToken(i).TokenType == TokenType.WhiteSpace)
                    return true;
            }

            return false;
        }

        private bool IsStrongAndItalicsSequenceRight(TokenType tokenType, int position, out int closeIndex)
        {
            closeIndex = 0;
            if (Tokens[position].TokenType == TokenType.Strong)
                return true;
            if (!TryGetFirstIndexOfTokenInLine(Tokens[position].TokenType, position, out closeIndex))
                return true; 
  
            if (!TryGetFirstIndexOfTokenInLine(tokenType, position, out var oppositeOpenIndex))
                return true;
            if (!TryGetFirstIndexOfTokenInLine(tokenType, oppositeOpenIndex, out var oppositeCloseIndex))
                return true;

            return !(position < oppositeOpenIndex && oppositeCloseIndex < closeIndex);
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

        public bool CanParse(TokenType tokenType) => tokenType is TokenType.Italics or TokenType.Strong;
    }
}