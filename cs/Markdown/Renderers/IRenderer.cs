using Markdown.Parsers;

namespace Markdown.Renderers
{
    public interface IRenderer
    {
        string Render(ParsedDocument parsedDocument);
    }
}
