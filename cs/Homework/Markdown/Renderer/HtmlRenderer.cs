using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Renderer
{
    public class HtmlRenderer : IRenderer
    {
        private HashSet<MarkdownToken> visitedTokens = new();

        public string Render(MarkdownToken[] tokens)
        {
            var result = tokens.Select(t => t.GetHtmlFormatted());
            return string.Join("", result);

        }

    }
}