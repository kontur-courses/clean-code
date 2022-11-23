using System.Collections.Generic;

namespace Markdown
{
    public class Parser
    {
        private Dictionary<string, Mod> separators;
        private Dictionary<Mod, string> htmlAnalogs;

        public Parser()
        {
            separators = new Dictionary<string, Mod>()
            {
                [""] = Mod.Common,
                ["#"] = Mod.Title,
                ["__"] = Mod.Bold,
                ["_"] = Mod.Italic,
                ["\\"] = Mod.Slash
            };

            htmlAnalogs = new Dictionary<Mod, string>()
            {
                [Mod.Title] = "h1",
                [Mod.Bold] = "strong",
                [Mod.Italic] = "em",
            };
        }

        public string ParseMdToHTML(string markdownText)
        {
            var tokenizer = new Tokenizer(separators);

            var toks = tokenizer.TikenizeText(markdownText);

            var analyzer = new TokenAnalyzer(markdownText);
            var verifyTokens = analyzer.LeadToSpecification(toks);

            var htmlBuilder = new HtmlBuilder(htmlAnalogs, markdownText);
            htmlBuilder.ConvertTokensToHtml(verifyTokens);

            var html = htmlBuilder.GetHtml();
            return html;
        }
    }
}
