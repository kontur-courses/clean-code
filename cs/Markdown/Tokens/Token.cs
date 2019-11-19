using System;

namespace Markdown.Tokens
{
    public class Token
    {
        public int Position { get; }
        public string Value { get; }
        public bool IsSeparator { get; }

        public Token(int position, string value, bool isSeparator)
        {
            if (position < 0)
                throw new ArgumentException($"position {position} was less than zero");
            Position = position;
            Value = value;
            IsSeparator = isSeparator;
        }
    }
}