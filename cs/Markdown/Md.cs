using System;

namespace Markdown
{
    public class Md
    {
        public IRenderer Renderer { get; }

        public Md(IRenderer renderer)
        {
            Renderer = renderer;
        }

        public string Render(string text)
        {
            var parser = new MdParser();
            var tokens = parser.ParseTokens(text);
            var result = Renderer.Render(tokens, text);

            return result;
        }
    }
}
