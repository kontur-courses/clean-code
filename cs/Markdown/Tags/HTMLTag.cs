using Markdown.DTOs;

namespace Markdown.Tags
{
    public class HTMLTag : ITag
    {
        public string StringTag { get; }
        public int Length { get; }
        public TagTypeEnum TagType { get; }
        public TagClassEnum TagClass { get; set; }
        public ITag Parent { get; set; }

        public HTMLTag(string tag, TagTypeEnum tagType)
        {
            StringTag = tag;
            TagType = tagType;
            this.Length = StringTag.Length;
        }
    }
}