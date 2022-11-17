namespace Markdown.Tags.Implementation;

public class Bold : ITag
{
    public string MarkdownName => "__";
    public string TranslateName => "!!!Bold!!!";
    public string HtmlName => "strong";
}