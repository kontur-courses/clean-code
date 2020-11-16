using System.Text;
using Markdown.Infrastructure.Formatters;
using Markdown.Infrastructure.Parsers;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdownText)
        {
            var tagValidator = new TagValidator(markdownText);
            var blockBuilder = new BlockBuilder(markdownText);
            var markdownParser = new MarkdownParser(tagValidator, blockBuilder);
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