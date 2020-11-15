namespace Markdown.Infrastructure.Parsers.Tags
{
    public class PictureTag : Tag
    {
        public readonly string Description;

        public PictureTag(string description) : base(Blocks.Style.Media)
        {
            Description = description;
        }
    }
}