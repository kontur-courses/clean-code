using System;
using System.Collections.Generic;
using Markdown.Models;

namespace Markdown
{
    public class MarkdownRenderer
    {
        private string text;

        public MarkdownRenderer(string text)
        {
            this.text = text;
        }

        public string RenderMatches(IEnumerable<TokenQueryMatch> matches) => throw new NotImplementedException();
    }
}