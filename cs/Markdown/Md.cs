using System.Collections.Generic;

namespace Markdown
{
    class Md
    {
        private readonly TextParser parser;
        private readonly HtmlCreator creator;

        public Md(TextParser parser, HtmlCreator creator)
        {
            this.parser = parser;
            this.creator = creator;
        }

        public string Render(string rawText)
        {
            var tokens = GetTokensFromText(rawText);
            return CreateHtmlFromTokens(tokens);
        }

        private IEnumerable<Token> GetTokensFromText(string text)
        {
            return parser.Parse(text);
        }

        private string CreateHtmlFromTokens(IEnumerable<Token> tokens)
        {
            return creator.CreateFromTokens(tokens);
        }
    }
}
