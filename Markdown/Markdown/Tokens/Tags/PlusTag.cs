namespace Markdown.Tokens.Tags;

public class PlusTag : ITag
{
    public ETagType Type => ETagType.PlusMarkedList;
    public string HtmlTag => "li"; 
}