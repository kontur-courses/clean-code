namespace Markdown.Tokens.Tags;

public class DashTag : ITag
{
    public ETagType Type => ETagType.DashMarkedList;
    public string HtmlTag => "li";
}