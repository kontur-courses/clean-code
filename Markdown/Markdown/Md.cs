namespace Markdown
{
    public class Md
    {
        public string Render(string md)
        {
            var mdParser = new Parser(md);
            var mdDocument = mdParser.Parse();
            return HtmlRender.RenderDocument(mdDocument);
        }
    }
}