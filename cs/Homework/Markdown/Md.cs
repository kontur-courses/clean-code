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
            var result = new List<string>();
            var tokens = parser.Parse(text);
            foreach (var token in tokens)
            {
                if(token is IMarkdownToken mdToken)
                    result.Add(mdToken.GetHtmlFormatted());
            }

            return string.Concat(result);
        }
    }
}