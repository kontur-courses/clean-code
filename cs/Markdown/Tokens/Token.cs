using System;

namespace Markdown.Tokens
{
    public class Token
    {
        public int Position { get; }
        public int Length { get; }
        public int EndPosition => Position + Length;

        private readonly string value;

        public string TokenValue => value.Substring(Position, Length);

        public Token(int position, int length, string value)
        {
            if (position < 0 || position + length > value.Length)
                throw new ArgumentException(
                    $"substring {position}, {position + length} is not in string with length {value.Length}");
            Position = position;
            Length = length;
            this.value = value;
        }
    }
}