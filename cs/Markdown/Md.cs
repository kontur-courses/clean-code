using System.Text;

namespace Markdown
{
    public class Md
    {
        private readonly Heading heading = new Heading();
        private readonly Bold bold = new Bold();
        private readonly Italic italic = new Italic();

        public string Render(string markdownText)
        {
            var htmlText = new StringBuilder();

            heading.GetHtmlTag(markdownText, 0);
            bold.GetHtmlTag(markdownText, 0);
            italic.GetHtmlTag(markdownText, 0);

            return htmlText.ToString();
        }
    }
}