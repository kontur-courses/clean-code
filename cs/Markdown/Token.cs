using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public TagType TagType { get; }
        public string Value { get; }
        public int Position { get; }
        public int Length { get; }
        public List<Token> NestedTokens { get; }

        public Token(TagType type, string value, int position)
        {
            TagType = type;
            Value = value;
            Position = position;
            Length = value.Length;
            NestedTokens = new List<Token>();
        }
    }
}
