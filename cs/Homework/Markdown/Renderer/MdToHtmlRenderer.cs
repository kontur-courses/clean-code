using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Renderer
{
    public class MdToHtmlRenderer : IRenderer<MarkdownToken>
    {
        public string Render(IEnumerable<MarkdownToken> tokens)
        {
            var result = tokens.Select(t => t.GetHtmlFormatted());
            return string.Concat(result);
        }
    }
}