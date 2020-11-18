using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        private readonly ITokenConverterFactory tokenConverters;
        private readonly IReadOnlyCollection<ITokenReader> tokenGetters;

        public Markdown(ITokenConverterFactory tokenConverters, IReadOnlyCollection<ITokenReader> tokenGetters)
        {
            this.tokenConverters = tokenConverters;
            this.tokenGetters = tokenGetters;
        }

        public string Render(string text)
        {
            var tokens = new TextParser(tokenGetters).GetTextTokens(text);
            return new HTMLConverter(tokenConverters).ConvertTokens(tokens);
        }
    }
}