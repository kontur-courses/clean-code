using System.Text;
using Markdown.Infrastructure.Formatters;
using Markdown.Infrastructure.Parsers.Markdown;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdownText)
        {
            var textHelper = new TextHelper(markdownText);
            var tagValidator = new TagValidator(textHelper);
            var blockBuilder = new BlockBuilder(textHelper);
            var markdownParser = new MarkdownParser(tagValidator, blockBuilder, textHelper);
            var block = markdownParser.Parse();

            var htmlFormatter = new HtmlFormatter();
            var htmlSentences = block.Format(htmlFormatter);

            var stringBuilder = new StringBuilder();
            foreach (var htmlSentence in htmlSentences)
                stringBuilder.Append(htmlSentence);

            return stringBuilder.ToString();
        }
    }
}