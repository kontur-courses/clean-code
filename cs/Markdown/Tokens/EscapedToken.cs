using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tokens
{
    public sealed record EscapedToken : Token
    {
        public char EscapedChar { get; }
        public EscapedToken(char escapedChar) { EscapedChar = escapedChar; }
    }
}
