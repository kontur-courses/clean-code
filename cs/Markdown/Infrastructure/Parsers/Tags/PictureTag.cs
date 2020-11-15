namespace Markdown.Infrastructure.Parsers.Tags
{
    public class PictureTag : Tag
    {
        public string Description;

        public PictureTag(string description) : base(Blocks.Style.Picture)
        {
            Description = description;
        }
    }
}