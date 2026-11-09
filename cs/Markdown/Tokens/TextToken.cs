using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tokens
{
    public sealed record TextToken : Token
    {
        public string Text { get; }
        public TextToken(string text) { Text = text; }
    }
}
