namespace Markdown.Tokens.Tags;

public class AsteriskTag : ITag
{
    public ETagType Type => ETagType.AsteriskMarkedList;
    public string HtmlTag => "li";
}