using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class ParserLink : Parser
    {
        public ParserLink(IEnumerable<IToken> tokens) : base(tokens)
        {
        }
        
        public override TokenTree ParseToken(int position)
        {
            var token = Tokens[position];
            var oppositeTokenType = GetOppositeTokenType(token.TokenType);
            if (!HasCloseTokenInLine(oppositeTokenType, position + 1))
                return new ParseAsText(Tokens).ParseToken(position);

            if (!HasLink(position, oppositeTokenType, out var closeIndex, out var oppositeCloseIndex))
                return new ParseAsText(Tokens).ParseToken(position);
            var name = new List<TokenTree>();
            position++;
            while (position < closeIndex && Tokens[position].TokenType != oppositeTokenType)
            {
                var component = base.ParseToken(position);
                name.Add(component);
                position += component.Count;
            }
            
            var text = new List<string>();
            for (var i = 1; i < 3; i++)
            {
                text.Add(NextToken(position + i).Value);
            }
            
            return new TokenTree(new TokenLink().Create(text.ToArray(), 0), name, 5);
        }

        private bool HasLink(int position, TokenType tokenType, out int closeIndex, out int oppositeCloseIndex)
        {
            var token = Tokens[position];
            oppositeCloseIndex = 0;
            if (!TryGetFirstIndexOfTokenInLine(TokenType.SquareBracketClose, position, out closeIndex))
                return false;
            if (!TryGetFirstIndexOfTokenInLine(TokenType.BracketOpen, position, out var oppositeOpenIndex))
                return false;
            if (!TryGetFirstIndexOfTokenInLine(TokenType.BracketClose, position, out oppositeCloseIndex, oppositeOpenIndex + 1))
                return false;

            return oppositeOpenIndex == closeIndex  + 1;
        }
        
        private TokenType GetOppositeTokenType(TokenType tokenType)
        {
            return tokenType switch
            {
                TokenType.BracketOpen => TokenType.BracketClose,
                TokenType.SquareBracketOpen => TokenType.SquareBracketClose,
                _ => throw new ArgumentException($"Unsupported tokenType: {tokenType}")
            };
        }
    }
}