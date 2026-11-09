using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tokens
{
    public sealed record LinkTailToken : Token
    {
        public string Url { get; }
        public bool Closed { get; }
        public LinkTailToken(string url, bool closed)
        {
            Url = url;
            Closed = closed;
        }
    }
}
