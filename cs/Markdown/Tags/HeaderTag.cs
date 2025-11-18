namespace Markdown.Tags;

public class HeaderTag : BlockTag
{
    public override string MdTag => "# ";
    public override string HtmlOpenTag => "<h1>";
    public override string HtmlCloseTag => "</h1>";
    public override string HtmlBlockOpenTag => "";
    public override string HtmlBlockCloseTag => "";

}