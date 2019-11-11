using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Core
{
    class Token
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
    }
}