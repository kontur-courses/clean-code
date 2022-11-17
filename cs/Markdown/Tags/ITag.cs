namespace Markdown.Tags;

public interface ITag
{
    public string MarkdownName { get; }
    public string TranslateName { get; }
    public string HtmlName { get; }
}