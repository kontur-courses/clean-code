namespace Markdown.Models.Tags
{
    internal class PairedTag
    {
        public Tag SourceTag { get; }
        public int Position { get; }
        public PairedTag Partner { get; set; }
        public bool IsOpening { get; set; }
        public bool IsClosing { get; set; }
        public PairedTag(Tag sourceTag, int position)
        {
            SourceTag = sourceTag;
            Position = position;
        }
    }
}
