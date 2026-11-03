using Markdown.Parsing;

namespace Markdown;

public static class Md
{
    public static string Render(string text)
    {
        var inlineParser = new InlineParser();
        var blockParser = new BlockParser(inlineParser);

        return blockParser.Parse(text);
    }
}