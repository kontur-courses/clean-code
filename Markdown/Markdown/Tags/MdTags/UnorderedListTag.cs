namespace Markdown.Tags.MdTags;

public class UnorderedListTag : SingleTag
{
    public override string MdTag => "* ";
    public override string HtmlTag => "<li>";
    public override string? HtmlContainer => "<ul>";
}