using System.Text.RegularExpressions;

namespace Markdown;

public class Md
{
    private readonly MarkdownParser _parser;
    private readonly IMdRenderer? _renderer;

    public Md(List<Tag> tags, IMdRenderer? renderer)
    {
        _parser = new MarkdownParser(tags);
        _renderer = renderer;
    }


    public string Render(string text)
    {
        _parser.Parse(text);

        var rendered = _renderer?.Render(_parser);
        return rendered ?? string.Empty;
    }
}