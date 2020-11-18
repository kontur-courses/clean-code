using System.Collections.Generic;

namespace Markdown
{
    public class Markdown
    {
        private readonly IReadOnlyCollection<ITokenGetter> tokenGetters;
        private readonly Dictionary<TokenType, string> tokensText;

        public Markdown()
        {
            tokensText = new Dictionary<TokenType, string>
            {
                {TokenType.Text, ""},
                {TokenType.Emphasized, "em"},
                {TokenType.Header, "h1"},
                {TokenType.Strong, "strong"}
            };
            tokenGetters = new ITokenGetter[]
            {
                new HeaderTokenGetter(),
                new StrongTokenGetter(),
                new EmphasizedTokenGetter(),
                new TextTokenGetter()
            };
        }

        public string Render(string text)
        {
            var tokens = new TextParser(tokenGetters).GetTextTokens(text);
            return new HTMLConverter(tokensText).GetHtml(tokens);
        }
    }
}