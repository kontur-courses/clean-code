using System.Linq;

namespace Markdown
{
    public class Md
    {
        private readonly MarkdownTextTokenizer tokenizer;
        private readonly ITokenConverter converter;

        public Md()
        {
            tokenizer = new MarkdownTextTokenizer();
            converter = new HtmlTokenConverter();
        }

        public string Render(string rawText)
        {
            var tokens = tokenizer.GetTokens(rawText).ToList();
            var renderedText = tokens.Select(t => converter.ConvertToken(t, rawText)).ToList();
            return string.Join("", renderedText);
        }
    }
}
