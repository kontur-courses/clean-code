namespace Markdown.Models.Tags.MdTags
{
    internal class Plus : Tag
    {
        public override string Opening => "+ ";
        public override string Closing => "\n";
    }
}
