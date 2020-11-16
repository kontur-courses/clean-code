using System.Text;
using Markdown.Infrastructure.Formatters;
using Markdown.Infrastructure.Parsers;

namespace Markdown
{
    public class Markdown : IRenderer
    {
        private readonly ITextHelper textHelper;
        private readonly IBlockParser blockParser;
        private readonly IBlockFormatter blockFormatter;

        public Markdown(ITextHelper textHelper, IBlockParser blockParser, IBlockFormatter blockFormatter)
        {
            this.textHelper = textHelper;
            this.blockParser = blockParser;
            this.blockFormatter = blockFormatter;
        }
        public string Render(string markdownText)
        {
            textHelper.Initialise(markdownText);

            var block = blockParser.Parse();
            var htmlSentences = block.Format(blockFormatter);
            var stringBuilder = new StringBuilder();
            foreach (var htmlSentence in htmlSentences)
                stringBuilder.Append(htmlSentence);

            return stringBuilder.ToString();
        }
    }
}