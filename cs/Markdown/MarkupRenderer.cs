using Markdown.TagConverter;
using Markdown.Parser;
using Markdown.Syntax;

namespace Markdown;

public class MarkupRenderer
{
    private ISyntax syntax;
    private IParser parser;
    private IConverter converter;

    public MarkupRenderer(ISyntax syntax, IParser parser, IConverter converter)
    {
        this.syntax = syntax;
        this.parser = parser;
        this.converter = converter;
    }

    public string Render(string text)
    {
        var tagTokens = parser.ParseTokens(text);
        var converter = new MarkupConverter(syntax);
        return converter.ConvertTags(tagTokens, text);
    }
}