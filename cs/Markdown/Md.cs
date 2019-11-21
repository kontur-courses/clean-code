using System.Collections.Generic;
using System.Linq;
using Markdown.Parser;
using Markdown.Parser.Tags;
using Markdown.Tools;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            var treeBuilder = new TreeBuilder();

            var tree = treeBuilder.ParseMarkdown(markdown);
            var html = tree.GetText();

            return html;
        }
    }
}