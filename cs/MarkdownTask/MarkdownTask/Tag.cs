namespace MarkdownTask
{
    public class Tag
    {
        public Tag(int startsAt, int length, TagType type)
        {
            StartsAt = startsAt;
            Length = length;
            Type = type;
        }

        public int StartsAt { get; set; }
        public int Length { get; set; }
        public TagType Type { get; set; }
    }
}