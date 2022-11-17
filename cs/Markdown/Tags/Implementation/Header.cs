namespace Markdown.Tags.Implementation;

public class Header : ITag
{
    public string MarkdownName => "#";
    public string TranslateName => "!!!Header!!!";
    public string HtmlName => "h1";
}