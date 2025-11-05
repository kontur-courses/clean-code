using Markdown.Parsing;

namespace Markdown;

public class Md
{
    private MarkdownParser markdownParser;

    public Md(MarkdownParser markdownParser)
    {
        this.markdownParser = markdownParser;
    }

    public string Render(string markdownText)
    {
        var markdownDocument = markdownParser.Parse(markdownText);
        return markdownDocument.ToHtml();
    }
}