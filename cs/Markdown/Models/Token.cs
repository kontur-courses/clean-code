using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Models
{
    internal class Token : IToken
    {
        public int StartPosition { get; }
        public int Length { get; }
        public string Value { get; }

        public Token(int startPosition, string value)
        {
            StartPosition = startPosition;
            Value = value;
            Length = value.Length;
        }
    }
}