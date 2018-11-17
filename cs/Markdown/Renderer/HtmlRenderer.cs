using System.Collections.Generic;
using Markdown.Tag.Standart;

namespace Markdown.Renderer
{
    public class HtmlRenderer : IRenderer
    {
        private readonly IReadOnlyDictionary<string, string> replaceRules;

        public HtmlRenderer()
        {
            replaceRules = new Dictionary<string, string>
            {
                {new Italic().Translation, "em"},
                {new Bold().Translation, "strong"}
            };
        }

        public string Render(string line)
        {
            foreach (var replaceRule in replaceRules)
            {
                var tagStart = $"<{replaceRule.Key}>";
                var tagEnd = $"</{replaceRule.Key}>";
                var htmlTagStart = $"<{replaceRule.Value}>";
                var htmlTagEnd = $"</{replaceRule.Value}>";
                line = line.Replace(tagStart, htmlTagStart);
                line = line.Replace(tagEnd, htmlTagEnd);
            }

            return $"<p>{line}</p>";
        }
    }
}