namespace Markdown
{
    public class Md
    {
        public string Renderer(string markdown)
        {
            var markdownParser = new MarkdownParser();
            var markdownDocument = new MarkdownDocument(markdownParser.Parse(markdown));
            return markdownDocument.ToHtml();
        }
    }
}