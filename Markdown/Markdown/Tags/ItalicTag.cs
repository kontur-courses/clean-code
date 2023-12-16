namespace Markdown;

public class ItalicTag : ITag
{
    public string HtmlTag => "em";
    public bool IsPaired => true;
    public string OpenTag => "_";
    public string CloseTag => "_";
}