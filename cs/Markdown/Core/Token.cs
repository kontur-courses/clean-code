using System;

namespace Markdown.Core
{
    class Token
    {
        public int StartPosition { get; }
        public string Value { get; }
        public int Length => Value.Length;

        public Token(int startPosition, string value)
        {
            if (startPosition < 0)
                throw new ArgumentException("start position must be non-negative");
            StartPosition = startPosition;
            Value = value;
        }
    }
}