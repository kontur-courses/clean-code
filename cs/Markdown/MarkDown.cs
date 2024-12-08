using Markdown.interfaces;
using Markdown.MarkDownConverter;
using Markdown.Parser;
using Markdown.Parser.Interface;

namespace Markdown;

public class MarkDown
{
    private readonly IMarkdownConverter _converter = new MarkdownConverter();
    private readonly IMarkdownParser _parser = new MarkdownParser();

    public string Render(string markdown)
    {
        var tokens = _parser.Parse(markdown);
        return _converter.Convert(tokens);
    }
}