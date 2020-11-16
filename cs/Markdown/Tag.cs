namespace Markdown
{
    public class Tag
    {
        public readonly int Position;
        public readonly TagInfo Name;
        public readonly int Length;

        public Tag(TagInfo name, int position, int length)
        {
            Position = position;
            Name = name;
            Length = length;
        }
    }
}