namespace Markdown;

public class Md
{
    public string Render(string markdown)
    {
        var blockNodes = MarkdownParser.Parse(markdown);

        var renderer = new HtmlRenderer();
        return renderer.Render(blockNodes);
    }
}