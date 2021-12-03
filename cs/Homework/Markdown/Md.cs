using System;
using System.Collections.Generic;
using Markdown.Parser;
using Markdown.Renderer;
using Markdown.Tokens;

namespace Markdown
{
    public static class Md
    {
        private static readonly TokenParser parser = new();
        private static readonly HtmlRenderer renderer = new();

        public static string Render(string text)
        {
            var result = new List<MarkdownToken>();
            var tokens = parser.Parse(text);

            return new HtmlRenderer().Render(tokens.ToArray());
        }
    }
}