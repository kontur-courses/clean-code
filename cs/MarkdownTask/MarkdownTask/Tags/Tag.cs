namespace MarkdownTask.Tags
{
    public class Tag
    {
        public Tag(int startsAt, int length, TagType type)
        {
            StartsAt = startsAt;
            Length = length;
            Type = type;
        }

        public int StartsAt { get; }
        public int Length { get; }
        public TagType Type { get; }
    }
}