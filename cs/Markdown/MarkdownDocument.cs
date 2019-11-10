using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkdownDocument
    {
        public List<Element> Elements { get; set; }

        public string ToHtml() => throw new NotImplementedException();
    }
}