using System.Text;
using Markdown.Parsers;

namespace Markdown.Renderers
{
    public class HtmlRenderer : IRenderer
    {
        public string Render(ParsedDocument parsedDocument)
        {
            var htmlText = new StringBuilder();
            foreach (var textBlock in parsedDocument.TextBlocks)
            {
                htmlText.Append(textBlock.ToHtml());
            }
            return htmlText.ToString();
        }
    }
}