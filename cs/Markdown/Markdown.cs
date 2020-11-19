namespace Markdown
{
    public class Markdown
    {
        private readonly IParser textParser;
        private readonly IConverter tokenConverter;

        public Markdown(IConverter tokenConverter, IParser textParser)
        {
            this.tokenConverter = tokenConverter;
            this.textParser = textParser;
        }

        public string Render(string text)
        {
            var tokens = textParser.GetTextTokens(text);
            return tokenConverter.ConvertTokens(tokens);
        }
    }
}