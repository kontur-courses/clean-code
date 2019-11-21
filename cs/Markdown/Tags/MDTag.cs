using Markdown.DTOs;

namespace Markdown.Tags
{
    public class MDTag : ITag
    {
        public string StringTag { get; }
        public int Length { get; }
        public TagTypeEnum TagType { get; }
        public TagClassEnum TagClass { get; set; }
        public ITag Parent { get; set; }

        public MDTag(string tag, TagTypeEnum tagType)
        {
            StringTag = tag;
            TagType = tagType;
            this.Length = StringTag.Length;
        }
        
        public MDTag(string tag, TagTypeEnum tagType, ITag parent) : this(tag, tagType)
        {
            Parent = parent;
        }

        public static MDTag GetTwoUnderscoreTag()
        {
            return new MDTag("__", TagTypeEnum.TwoUnderScoreMd, GetOneUnderscoreTag());
        }

        public static MDTag GetOneUnderscoreTag()
        {
            return new MDTag("_", TagTypeEnum.OneUnderscoreMd);
        }
    }
}