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
            var tags = new List<MarkdownTag> {new BoldTag(), new ItalicTag()};
            var classifier = new CharClassifier(tags.SelectMany(t => t.String));
            var treeBuilder = new TreeBuilder(tags, classifier);

            var tree = treeBuilder.ParseMarkdown(markdown);
            var html = tree.GetText();

            return html;
        }
    }
}