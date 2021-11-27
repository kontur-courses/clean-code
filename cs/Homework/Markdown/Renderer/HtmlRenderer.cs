using System;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Renderer
{
    public class HtmlRenderer : IRenderer
    {
        public string Render(IMarkdownToken[] tokens)
        {
            var htmlText = tokens.Select(t => t.GetHtmlFormatted());
            return string.Join("", htmlText);
        }
    }
}