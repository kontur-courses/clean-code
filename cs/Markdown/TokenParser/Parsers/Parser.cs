using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class Parser
    {
        protected readonly IReadOnlyList<IToken> Tokens;
        
        public Parser(IEnumerable<IToken> tokens)
        {
            Tokens = tokens.ToList();
        }
        
        public TokenTree[] Parse()
        {
            var components = new List<TokenTree>();
            var position = 0;
            while (position < Tokens.Count)
            {
                var component = ParseToken(position);
                position += component.Count;
                components.Add(component);
            }

            return components.ToArray();
        }

        public virtual TokenTree ParseToken(int position)
        {
            var token = Tokens[position];
            return token.TokenType switch
            {
                TokenType.Header1 => new ParseHeader(Tokens).ParseToken(position),
                TokenType.Text or TokenType.NewLine => new ParseAsText(Tokens).ParseToken(position),
                TokenType.Italics or TokenType.Strong => new ParseUnderscore(Tokens).ParseToken(position),
                _ => throw new ArgumentOutOfRangeException($"Unsupported tokenType: {token.TokenType}")
            };
        }
        
        protected IToken NextToken(int position) => GetTokenWithOffset(position, 1);
        
        protected IToken PreviousToken(int position) => GetTokenWithOffset(position, -1);
        
        protected bool HasCloseTokenInLine(TokenType tokenType, int position) => 
            TryGetFirstIndexOfTokenInLine(tokenType, position, out _);

        protected bool TryGetFirstIndexOfTokenInLine(TokenType tokenType, int position, out int index, int offset = 1)
        {
            index = offset;
            do
            {
                var currentToken = GetTokenWithOffset(position, index);
                if (currentToken.TokenType == tokenType)
                    return true;
                index++;
            } while (position + index < Tokens.Count && Tokens[position + index].TokenType != TokenType.NewLine);

            return false;
        }

        private IToken GetTokenWithOffset(int currentPosition, int offset)
        {
            var index = currentPosition + offset;
            if (index >= Tokens.Count)
                return Tokens[^1];
            if (index <= 0)
                return Tokens[0];
            return Tokens[index];
        }
    }
}