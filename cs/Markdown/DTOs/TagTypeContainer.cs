using Markdown.Tags;

namespace Markdown.DTOs
{
    public enum TagTypeEnum
    {
        TwoUnderScoreMd,
        OneUnderscoreMd,
        StrongHtml,
        EmHtml
    }

    public enum TagClassEnum
    {
        Opener = 0,
        Closer = 1
    }

    public class TagTypeContainer
    {
        public readonly int position;
        public readonly ITag Tag;
        public TagTypeEnum TagType => Tag.TagType;
        public TagClassEnum TagClass => Tag.TagClass;
        public int Length => Tag.Length;

        public TagTypeContainer(ITag tag, bool tagClass, int position)
        {
            switch (tagClass)
            {
                case true:
                    tag.TagClass = TagClassEnum.Closer;
                    break;
                case false:
                    tag.TagClass = TagClassEnum.Opener;
                    break;
            }

            this.position = position;
            this.Tag = tag;
        }


        public static bool operator ==(TagTypeContainer t1, TagTypeContainer t2)
        {
            return t1.Tag.TagType == t2.Tag.TagType && t1.Tag.TagClass == t2.Tag.TagClass;
        }

        public static bool operator !=(TagTypeContainer t1, TagTypeContainer t2)
        {
            return t1.Tag.TagType != t2.Tag.TagType && t1.Tag.TagClass != t2.Tag.TagClass;
        }
    }
}