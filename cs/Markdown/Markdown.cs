using Markdown.TokenSearcher;
using Markdown.Converter;
namespace Markdown
{
    public class Markdown
    {
        private ITokenSearcher tokenSearcher;
        private IHtmlConverter htmlConverter;
        public Markdown(ITokenSearcher tokenSearcher, IHtmlConverter htmlConverter)
        {
            this.tokenSearcher = tokenSearcher;
            this.htmlConverter = htmlConverter;
        }

        public string Render(string markdownText)
        {
            if (string.IsNullOrEmpty(markdownText))
                throw new ArgumentNullException("The string must not be empty");
            var tokens = tokenSearcher.FindTokens(markdownText);
            var htmlText = htmlConverter.ConvertFromMarkdownToHtml(tokens);
            return htmlText;
        }

    }
}
