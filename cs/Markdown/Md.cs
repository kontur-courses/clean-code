using Markdown.Parsers.MdParsers;
using Markdown.Renderers;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Markdown.Tests")]
namespace Markdown
{
    internal class Md(ParserMd parser, IRenderer renderer)
    {
        public readonly ParserMd Parser = parser ?? throw new ArgumentNullException(
                "Передаваемый ParserMd не может быть null.");

        public readonly IRenderer Renderer = renderer ?? throw new ArgumentNullException(
                "Передаваемый IRenderer не может быть null.");

        public string Render(string text)
        {
            var mdTokens = Parser.Parse(text);
            foreach (var token in mdTokens)
            {
                token.Render(Renderer);
            }
            return Renderer.ToString()!;
        }
    }
}
