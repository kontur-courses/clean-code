namespace Markdown.Tags
{
    public class Tag : ITag
    {
        public string Opening { get; set; }
        public string Closing { get; set; }
        public TagType Type { get; set; }

        public Tag(TagType type, string opening, string closing)
        {
            Opening = opening;
            Type = type;
            Closing = closing;
        }
    }
}