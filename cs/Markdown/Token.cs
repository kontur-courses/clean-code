using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public int Position { get; set; }
        public string Value { get; set; }
        public int Length => Value.Length;
        public TokenType Type { get; set; }
        public List<Token> ChildTokens { get; set; }

        public Token(int position, string value, TokenType type)
        {
            Value = value;
            Position = position;
            Type = type;
        }

        public string ValueWithoutTags()
        {
            return Type switch
            {
                TokenType.Heading => Value.Substring(1),
                TokenType.Emphasized => Value.Substring(1, Value.Length - 2),
                TokenType.Strong => Value.Substring(2, Value.Length - 4),
                _ => Value
            };
        }
    }
}