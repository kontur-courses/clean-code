using Markdown.Convert;
using Markdown.Parse;
using Markdown.Render;

namespace Markdown;

public class Md : IMarkupRenderer
{
    private readonly ITagConverter converter;

    public Md(ITagConverter converter)
    {
        this.converter = converter;
    }
    
    public string Render(string text)
    {
        var renderer = new TokenRenderer();
        var parser = new MarkdownMarkupParser();

        return renderer.Render(parser.Parse(text), converter);
    }
}