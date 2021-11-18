namespace Markdown.Tags
{
    public class ItalicsTag : Tag
    {
        public override string MdTag => "_";
        public override string HtmlTag => "em";
    }
}