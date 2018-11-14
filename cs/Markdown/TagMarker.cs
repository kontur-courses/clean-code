namespace Markdown
{
    public class TagMarker
    {
        public int Position { get; }
        public TagKeeper TagKeeper { get; }

        public TagMarker(TagKeeper tagKeeper, int position)
        {
            TagKeeper = tagKeeper;
            Position = position;
        }
    }
}
