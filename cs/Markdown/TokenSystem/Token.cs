using System;

namespace Markdown.TokenSystem
{
    public class Token
    {
        public int Position { get; }
        public int Length { get; }
        public string Value { get; }

        public Token(int position, int length, string value)
        {
            Position = position;
            Length = length;
            Value = value;
        }

        public static Token operator +(Token token1, Token token2)
        {
            return new Token(
                Math.Min(token1.Position, token2.Position), 
                token1.Length + token2.Length, 
                token1.Value + token2.Value);
        }
    }
}