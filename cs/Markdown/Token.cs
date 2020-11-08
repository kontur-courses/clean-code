using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public int Position { get; set; }
        public int Lenght => Value.Length;
        public string Value { get; private set; }
        public TokenType Type { get; private set; }
    }
}
