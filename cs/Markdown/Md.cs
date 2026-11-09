using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string text)
        {
            var rawBlocks = RawBlockParser.Parse(text);
            var renderedBlocks = rawBlocks.Select(InlineParser.Parse).ToArray();
            return HtmlRenderer.Render(renderedBlocks);
        }
    }
}
