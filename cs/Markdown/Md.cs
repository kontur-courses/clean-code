using Markdown.Parser;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdown)
        {
            var tree = TreeBuilder.ParseMarkdown(markdown);
            var html = tree.GetText();

            return html;
        }
    }
}