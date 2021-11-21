using Markdown.Parser;
using Markdown.Renderer;

namespace Markdown
{
    public class Md
    {
        public IMdParser Parser { get; }
        public IRenderer Renderer { get; }

        public Md(IRenderer renderer)
        {
            Renderer = renderer;
            Parser = new MdParser();
        }

        public Md(IRenderer renderer,IMdParser parser)
        {
            Renderer = renderer;
            Parser = parser;
        }

        public string Render(string text)
        {
            var tokens = Parser.ParseTokens(text);
            var result = Renderer.Render(tokens, text);

            return result;
        }
    }
}