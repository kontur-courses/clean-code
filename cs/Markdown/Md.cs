using Markdown.Parser;
using Markdown.Renderer;

namespace Markdown
{
    public class Md
    {
        public IRenderer Renderer { get; }
        private readonly MdParser parser;

        public Md(IRenderer renderer)
        {
            Renderer = renderer;
            parser = new MdParser();
        }

        public Md(IRenderer renderer, MdParser parser)
        {
            Renderer = renderer;
            this.parser = parser;
        }

        public string Render(string text)
        {
            var tokens = parser.ParseTokens(text);
            var result = Renderer.Render(tokens, text);

            return result;
        }
    }
}
