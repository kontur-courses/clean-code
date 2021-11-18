namespace MarkdownTask.Tags
{
    public class Tag
    {
        public Tag(TagType tagType, string openingPart, string closingPart, string content = default)
        {
            Type = tagType;
            TagContent = content;
            OpeningPart = openingPart;
            ClosingPart = closingPart;
        }

        public TagType Type { get; }
        public string TagContent { get; set; }
        public string OpeningPart { get; }
        public string ClosingPart { get; }
    }
}