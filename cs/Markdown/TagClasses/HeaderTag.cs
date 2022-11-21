using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.TagClasses
{
    public class HeaderTag : ITag
    {
        public int Position { get; set; }
        public int Length { get; set; }
        public bool IsEscaped { get; set; }
        public TagType Type { get; set; }

        public int CompareTo(object? obj)
        {
            if (obj == null) return 1;
            if (obj is ITag otherTagInfo)
                return Position.CompareTo(otherTagInfo.Position);
            throw new ArgumentException("Object is not a ITag");
        }

        public HeaderTag(int pos, TagType type, bool isEscaped = false)
        {
            Position = pos;
            Type = type;
            IsEscaped = isEscaped;
            Length = 0;
        }

        public string GetHtmlTag()
        {
            return "";
        }
    }
}