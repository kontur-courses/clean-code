using Markdown.Parsers;
using System;

namespace Markdown
{
    public class Md
    {
        private readonly BlockParser blockParser = new();
        private readonly HtmlRenderer renderer = new();

        public string Render(string markdown)
        {
            var blocks = blockParser.ParseToBlocks(markdown);
            return renderer.RenderDocument(blocks);
        }
    }
}
