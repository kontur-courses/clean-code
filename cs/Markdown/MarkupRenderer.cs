using Markdown.TagConverter;
using Markdown.Parser;
using Markdown.Syntax;

namespace Markdown;

public class MarkupRenderer
{
    private readonly IParser parser;
    private readonly IConverter converter;

    public MarkupRenderer(IParser parser, IConverter converter)
    {
        this.parser = parser;
        this.converter = converter;
    }

    public string Render(string text)
    {
        var tagTokens = parser.ParseTokens(text);
        return converter.ConvertTags(tagTokens, text);
    }
}