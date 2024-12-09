using Markdown.Converter;
using Markdown.Extensions;
using Markdown.TokenParser.Interfaces;

namespace Markdown;

public class Md
{
    private readonly IConverter converter;
    private readonly ITokenLineParser markdownTokenizer;

    public Md(ITokenLineParser tokenizer, IConverter converter)
    {
        markdownTokenizer = tokenizer;
        this.converter = converter;
    }

    public string Render(string mdString)
    {
        return converter.Convert(mdString.SplitIntoLines()
            .Select(markdownTokenizer.ParseLine)
            .ToArray());
    }
}