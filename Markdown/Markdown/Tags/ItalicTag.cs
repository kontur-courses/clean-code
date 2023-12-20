namespace Markdown;

public class ItalicTag : ITag
{
    public string MdTag => "_";
    public bool IsPaired => true;
    public HtmlTag HtmlTag => new() {Open = "<em>", Close = "</em>"};
}