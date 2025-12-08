using MarkupConverter.Parsers;
using MarkupConverter.Renderers;

namespace MarkupConverter;

public class MarkupConverter
{
    private readonly IParser parser;
    private readonly IRenderer renderer;

    public MarkupConverter(IParser parser, IRenderer renderer)
    {
        this.parser = parser;
        this.renderer = renderer;
    }

    public string Convert(string text)
    {
        var ast = parser.Parse(text);
        var markup = renderer.Render(ast);

        return markup;
    }
}