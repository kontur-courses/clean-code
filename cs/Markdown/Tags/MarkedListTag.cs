namespace Markdown.Tags;

public class MarkedListTag : BlockTag
{
    public override string MdTag => "* ";
    public override string HtmlOpenTag => "<li>";
    public override string HtmlCloseTag => "</li>";
    public override string HtmlBlockOpenTag => "<ul>";
    public override string HtmlBlockCloseTag => "</ul>";
}