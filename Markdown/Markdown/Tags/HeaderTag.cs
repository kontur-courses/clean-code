namespace Markdown;

public class HeaderTag : ITag
{
    public string MdTag => "# ";
    public bool IsPaired => false;
    public HtmlTag HtmlTag => new() {Open = "<h1>", Close = "</h1>"};
}