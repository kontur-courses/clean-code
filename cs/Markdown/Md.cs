using System.Collections.Generic;

namespace Markdown
{
    internal class Md
    {
        private readonly HtmlCreator creator;

        public Md(HtmlCreator creator)
        {
            this.creator = creator;
            
        }

        public string Render(string rawText)
        {
            var tokens = GetTokensFromText(rawText);
            return CreateHtmlFromTokens(tokens);
        }

        private static IEnumerable<Token> GetTokensFromText(string text) =>
            TextParser.For(text)
                      .Parse();

        private string CreateHtmlFromTokens(IEnumerable<Token> tokens) => creator.CreateFromTokens(tokens);
    }
}
