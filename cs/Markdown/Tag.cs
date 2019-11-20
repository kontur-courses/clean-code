namespace Markdown
{
    public interface ITag
    {
        string StringTag { get; }
        int Length { get; }
        TagTypeEnum TagType { get; }
        TagClassEnum TagClass { get; set; }
    }

    public class MDTag : ITag
    {
        public string StringTag { get; }
        public int Length { get; }
        public TagTypeEnum TagType { get; }
        public TagClassEnum TagClass { get; set; }

        public MDTag(string tag, TagTypeEnum tagType)
        {
            StringTag = tag;
            TagType = tagType;
            this.Length = StringTag.Length;
        }

        public static MDTag GetTwoUnderscoreTag()
        {
            return new MDTag("__", TagTypeEnum.TwoUnderScoreMd);
        }

        public static MDTag GetOneUnderscoreTag()
        {
            return new MDTag("_", TagTypeEnum.OneUnderscoreMd);
        }
    }

    public class HTMLTag : ITag
    {
        public string StringTag { get; }
        public int Length { get; }
        public TagTypeEnum TagType { get; }
        public TagClassEnum TagClass { get; set; }

        public HTMLTag(string tag, TagTypeEnum tagType)
        {
            StringTag = tag;
            TagType = tagType;
            this.Length = StringTag.Length;
        }
    }
}