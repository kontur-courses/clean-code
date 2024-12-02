using Markdown.Readers;

namespace Markdown;

public class Md
{
    public string Render(string markdown)
    {
        var reader = new MarkdownToHtmlConverter();
        return reader.ConvertMarkdownToHtml(markdown);
    }
}