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
            foreach (var replaceRool in replaceRules)
            {
                var tagStart = $"<{replaceRool.Key}>";
                var tagEnd = $"</{replaceRool.Key}>";
                var htmlTagStart = $"<{replaceRool.Value}>";
                var htmlTagEnd = $"</{replaceRool.Value}>";
                line = line.Replace(tagStart, htmlTagStart);
                line = line.Replace(tagEnd, htmlTagEnd);
            }

            return $"<p>{line}</p>";
        }
    }
}