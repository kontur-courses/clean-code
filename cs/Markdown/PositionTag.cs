using Markdown.Tags;

namespace Markdown
{
    public class PositionTag
    {
        public readonly Tag Tag;
        public readonly int Position;

        public PositionTag(Tag tag, int position)
        {
            Tag = tag;
            Position = position;
        }
    }
}
