using MarkupConverter.Parsers;
using MarkupConverter.Renderers;

namespace MarkupConverter;

public class MarkupConverter
{
    private readonly IParser _parser;
    private readonly IRenderer _renderer;

    public MarkupConverter(IParser parser, IRenderer renderer)
    {
        _parser = parser;
        _renderer = renderer;
    }

    public string Convert(string text)
    {
        var ast = _parser.Parse(text);
        var markup =  _renderer.Render(ast);
        
        return markup;
    }
}