namespace Markdown.Tags
{
    public class BoldTag : Tag
    {
        public override string MdTag => "__";
        public override string HtmlTag => "strong";
    }
}