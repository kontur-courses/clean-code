using Markdown.Generators;
using Markdown.Parsers;

namespace Markdown;

public class MarkdownToHtmlConverter
{
    private readonly IMarkingGenerator htmlGenerator;
    private readonly IMarkingParser markdownParser;

    public MarkdownToHtmlConverter(IMarkingParser markingParser, IMarkingGenerator markingGenerator)
    {
        markdownParser = markingParser;
        htmlGenerator = markingGenerator;
    }

    public string Convert(string text)
    {
        var tokens = markdownParser.ParseText(text);
        var htmlResult = htmlGenerator.Generate(tokens, text);
        return htmlResult;
    }
}
