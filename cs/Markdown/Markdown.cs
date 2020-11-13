using System.Text;
using Markdown.Infrastructure.Formatters;
using Markdown.Infrastructure.Parsers;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdownText)
        {
            var markdownParser = new MarkdownParser();
            var block = markdownParser.Parse(markdownText);

            var htmlFormatter = new HtmlFormatter();
            var htmlSentences = block.Format(htmlFormatter);

            var stringBuilder = new StringBuilder();
            foreach (var htmlSentence in htmlSentences)
                stringBuilder.Append(htmlSentence);
            
            return stringBuilder.ToString();
        }
    }
}