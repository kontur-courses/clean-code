using System.Text.RegularExpressions;
using Markdown.Parsing;
using Markdown.Render;

namespace Markdown;

public class Md
{
    private readonly MarkdownParser _parser;
    private readonly IMdRenderer? _renderer;

    public Md(List<IMdTag> tags, IMdRenderer? renderer)
    {
        _parser = new MarkdownParser(tags);
        _renderer = renderer;
    }


    public string Render(string text)
    {
        var document = _parser.ParseToDocument(text);

        var rendered = _renderer?.Render(document);
        return rendered ?? string.Empty;
    }
}