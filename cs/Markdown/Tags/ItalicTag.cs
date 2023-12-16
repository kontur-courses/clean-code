namespace Markdown.Tags
{
    public class ItalicTag : ITag
    {
        public TagType Type { get; } = TagType.Italic;
        public string StartTag { get; } = "_";
        public string? EndTag { get; } = "_";
        public int Position { get; }

        public ItalicTag(int position)
        {
            Position = position;
        }
    }
}
