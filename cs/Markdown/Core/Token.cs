using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Core
{
    class Token
    {
        public int StartPosition { get; }
        public string Value { get; }
        public int EndPosition => StartPosition + Length;
        public int Length => Value.Length;

        public Token(int startPosition, int length, string value)
        {
            StartPosition = startPosition;
            Value = value;
        }
    }
}