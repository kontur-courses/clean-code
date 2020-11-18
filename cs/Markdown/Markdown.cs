using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        private readonly IReadOnlyCollection<ITokenReader> tokenGetters;
        private readonly Dictionary<TokenType, ITagTokenConverter> tokensText;

        public Markdown()
        {
            tokensText = new Dictionary<TokenType, ITagTokenConverter>
            {
                {TokenType.Text, new TextTokenConverter()},
                {TokenType.Emphasized, new EmphasizeTokenConverter()},
                {TokenType.Header, new HeaderTokenConverter()},
                {TokenType.Strong, new StrongTokenConverter()}
            };
            tokenGetters = new ITokenReader[]
            {
                new HeaderTokenReader(),
                new StrongTokenReader(),
                new EmphasizedTokenReader(),
                new TextTokenReader()
            };
        }

        public string Render(string text)
        {
            var tokens = new TextParser(tokenGetters).GetTextTokens(text);
            return new HTMLConverter(tokensText).ConvertTokens(tokens);
        }
    }
}