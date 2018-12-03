using System.Collections.Generic;

namespace Markdown
{
    internal class Md
    {
        private readonly HtmlCreator creator = new HtmlCreator(new Dictionary<string, (string opening, string closing)>
        {
            ["_"] = ("<em>", "</em>"),
            ["*"] = ("<em>", "</em>"),
            ["__"] = ("<strong>", "</strong>"),
            ["**"] = ("<strong>", "</strong>"),
            ["___"] = ("<em><strong>", "</strong></em>"),
            ["***"] = ("<em><strong>", "</strong></em>")
        });

        public Md(HtmlCreator creator = null)
        {
            this.creator = creator ?? this.creator;
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
