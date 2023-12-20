namespace Markdown;

public class BoldTag : ITag
{
    public string MdTag => "__";
    public bool IsPaired => true;
    public HtmlTag HtmlTag => new() {Open = "<strong>", Close = "</strong>"};
}