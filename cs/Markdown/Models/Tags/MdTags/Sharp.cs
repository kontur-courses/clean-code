namespace Markdown.Models.Tags.MdTags
{
    internal class Sharp : Tag
    {
        public override string Opening => "# ";
        public override string Closing => "\n";
    }
}
