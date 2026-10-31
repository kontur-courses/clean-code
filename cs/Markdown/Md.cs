using Markdown.Parsers;

namespace Markdown;

public class Md
{
    public string Render(string markdown)
    {
        var inlineParser = new InlineParser();
        var blockParser = new BlockParser(inlineParser);
        var renderer = new HtmlRenderer();

        var blockNodes = blockParser.Parse(markdown);

        return renderer.Render(blockNodes);
    }
}