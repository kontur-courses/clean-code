using Markdown.Converters;
using Markdown.Tokens;

namespace Markdown.Core
{
    public class MarkdownProcessor
    {
        private readonly IConverter converter;
        private readonly ITokenReader tokenReader;

        public MarkdownProcessor(IConverter converter, ITokenReader tokenReader)
        {
            this.converter = converter;
            this.tokenReader = tokenReader;
        }

        public string Render(string markdownText)
        {
            var tokens = tokenReader.ReadAllTokens(markdownText);
            var htmlString = converter.Convert(tokens, markdownText);
            return htmlString;
        }
    }
}