using System.Collections.Generic;

namespace Markdown.Renderer
{
    public class HtmlRenderer : IRenderer
    {
        private Dictionary<string, string> replaceRules;

        public HtmlRenderer()
        {
            replaceRules = new Dictionary<string, string>
            {
                {"italic", "em"},
                {"bold", "strong"}
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