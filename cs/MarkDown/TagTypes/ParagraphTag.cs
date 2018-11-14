namespace MarkDown.TagTypes
{
    public class ParagraphTag : TagType
    {
        public ParagraphTag() : base("\n", "p", new TagType[]{new StrongTag(), new EmTag()}) {}
    }
}