using Markdown.Parser;
using Markdown.Renderer;

namespace Markdown
{
    public class Md
    {
        private static readonly TokenParser parser = new();
        private static readonly HtmlRenderer renderer = new();

        public static string Render(string text)
        {
            var tokens = parser.Parse(text);
            return renderer.Render(tokens.ToArray());
        }
    }
}