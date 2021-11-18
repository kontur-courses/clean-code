namespace Markdown.Tags
{
    public class HeaderTag : Tag
    {
        public override string MdTag => "#";
        public override string HtmlTag => "h1";
    }
}