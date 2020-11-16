using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public int Position { get; }
        public string Value { get; }
        public int Length => Value.Length;
        public int EndPosition => Position + Length - 1;
        public TokenType Type { get; }
        public List<Token> ChildTokens { get; set; }

        public Token(int position, string value, TokenType type)
        {
            Value = value;
            Position = position;
            Type = type;
        }

        public string GetValueWithoutTags()
        {
            return Type switch
            {
                TokenType.Heading => Value[1..],
                TokenType.Emphasized => Value[1..^1],
                TokenType.Strong => Value[2..^2],
                _ => Value
            };
        }
    }
}