using Markdown.Converters;

namespace Markdown
{
    public class Markdown
    {
        private readonly IConverter converter;
        private readonly ITextParser parser;

        public Markdown(ITextParser parser, IConverter converter)
        {
            this.parser = parser;
            this.converter = converter;
        }

        public string Render(string markdown)
        {
            var tokens = parser.GetTokens(markdown);

            return converter.ConvertTokens(tokens);
        }
    }
}