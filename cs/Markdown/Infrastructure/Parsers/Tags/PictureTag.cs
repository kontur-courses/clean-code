using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers.Tags
{
    public class PictureTag : Tag
    {
        public readonly string Description;

        public PictureTag(string description) : base(Style.Media)
        {
            Description = description;
        }
    }
}