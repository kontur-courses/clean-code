namespace Markdown
{
    public class Md
    {
        public string Render(string markdownText)
        {
            var parser = new Parser();
            return parser.ParseMdToHTML(markdownText);
        }
    }
}
