namespace Markdown
{
    public interface ITag
    {
        string StringTag { get; }
        int Length { get; }
        TagTypeEnum TagType { get; }
        TagClassEnum TagClass { get; set; }
    }

    public class TwoUnderscoreTag : ITag
    {
        public string StringTag => "__";
        public int Length { get; }
        public TagTypeEnum TagType { get; }
        public TagClassEnum TagClass { get; set; }

        public TwoUnderscoreTag()
        {
            TagType = TagTypeEnum.TwoUnderScore;
            this.Length = StringTag.Length;
        }
    }

    public class OneUnderscoreTag : ITag
    {
        public string StringTag => "_";
        public int Length { get; }
        public TagTypeEnum TagType { get; }
        public TagClassEnum TagClass { get; set; }

        public OneUnderscoreTag()
        {
            TagType = TagTypeEnum.TwoUnderScore;
            this.Length = StringTag.Length;
        }
    }
}