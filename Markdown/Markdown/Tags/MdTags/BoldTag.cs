namespace Markdown;

public class BoldTag : ITag
{
    public string MdTag => "__";
    public string HtmlTag => "<strong>";
}