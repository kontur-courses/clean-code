namespace Markdown;

public class HeaderTag : ITag
{
    public string MdTag => "# ";
    public string HtmlTag => "<h1>";
}