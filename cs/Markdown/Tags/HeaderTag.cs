namespace Markdown.Tags
{
    public class HeaderTag : Tag
    {
        public override bool IsPairMdTag => false;
        public override string MdTag => "#";
        public override string HtmlTag => "h1";
    }
}