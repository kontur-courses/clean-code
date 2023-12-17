namespace Markdown;

public class Tag
{
    public string Name { get; private set; }
    public string? MarkdownOpening { get; private set; }
    public string? MarkdownClosing { get; private set; }
    public string? HtmlKeyword { get; private set; }
    public string TagOpen => $"<{HtmlKeyword}>"; 
    public string TagClose => $"</{HtmlKeyword}>";

    public Tag(string name, string markdownOpening, string markdownClosing, string htmlKeyword)
    {
        Name = name;
        MarkdownOpening = markdownOpening;
        MarkdownClosing = markdownClosing;
        HtmlKeyword = htmlKeyword;
    }

    public override string ToString()
    {
        return Name;
    }
}