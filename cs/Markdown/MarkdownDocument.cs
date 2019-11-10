using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownDocument
    {
        public List<Token> Tokens { get; set; }

        public string ToHtml() => throw new NotImplementedException();
    }
}