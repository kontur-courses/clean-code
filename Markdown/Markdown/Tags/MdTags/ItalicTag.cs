namespace Markdown;

public class ItalicTag : ITag
{
    public string MdTag => "_";
    public string HtmlTag => "<em>";
}