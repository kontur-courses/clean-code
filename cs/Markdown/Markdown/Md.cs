namespace Markdown
{
    public class Md
    {
        public string Render(string md)
        {
            var mdParser = new MarkdownParser(md);
            var mdDocument = mdParser.Parse();
            return HtmlRender.RenderDocument(mdDocument);
        }
    }
}