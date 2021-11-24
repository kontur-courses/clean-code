using System.Linq;

namespace HtmlConvertor
{
    public class HtmlConvertor
    {
        private readonly Tokenizer tokenizer = new Tokenizer();
        private readonly TagValidator tagValidator = new TagValidator();
        private readonly MarkdownProcessor markdownProcessor = new MarkdownProcessor();

        public string ConvertMarkdownToHtml(string input)
        {
            var tokens = tokenizer.GetTokens(input).ToList();
            var validTokens = tagValidator.GetValidTokens(tokens).ToList();
            var htmlCode = markdownProcessor.Render(validTokens);

            return htmlCode;
        }
    }
}