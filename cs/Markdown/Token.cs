using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public Token(TokenInformation data, TokenType type, int position)
        {
            Data = data;
            TokenType = type;
            Position = position;
        }

        public TokenInformation Data { get; }
        public TokenType TokenType { get; }
        public int Position { get; }

        public override bool Equals(object obj)
        {
            var token = obj as Token;
            return token != null &&
                   EqualityComparer<TokenInformation>.Default.Equals(Data, token.Data) &&
                   TokenType == token.TokenType &&
                   Position == token.Position;
        }
    }
}