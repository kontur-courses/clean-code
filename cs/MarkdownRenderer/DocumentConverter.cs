using MarkdownRenderer.Abstractions;
using MarkdownRenderer.Implementations;

namespace MarkdownRenderer;

public class DocumentConverter
{
    private readonly DocumentParser _documentParser;
    private readonly Dictionary<Type, IElementRenderer> _renderers;


    public DocumentConverter(IEnumerable<IElementParser> parsers, IEnumerable<IElementRenderer> renderers)
    {
        _documentParser = new DocumentParser(parsers);
        _renderers = renderers.ToDictionary(
            renderer => renderer.RenderingElementType,
            renderer => renderer
        );
    }

    public string Convert(string source)
    {
        var element = _documentParser.ParseContentLine(source);
        return _renderers[element.GetType()].Render(element, _renderers);
    }
}