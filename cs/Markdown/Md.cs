namespace Markdown;

public class Md
{
    public string Render(string markdown)
    {
        var parser = new MarkdownParser(new InlineTokenizer());
        var blockNodes = parser.Parse(markdown);

        var renderer = new HtmlRenderer();
        return renderer.Render(blockNodes);
    }
}