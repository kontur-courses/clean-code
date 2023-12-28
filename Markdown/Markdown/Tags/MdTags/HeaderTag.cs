namespace Markdown;

public class HeaderTag : SingleTag
{
    public override string MdTag => "# ";
    public override string HtmlTag => "<h1>";
}