using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.TagClasses
{
    internal class TextTag : ITextTag
    {
        public string Content { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
        public bool IsEscaped { get; set; }
        public TagType Type { get; set; }

        public TextTag(int pos, TagType type, int length, bool isEscaped = false, string content = "")
        {
            Position = pos;
            Content = content;
            Type = type;
            IsEscaped = isEscaped;
            Length = length;
        }

        public int CompareTo(object? obj)
        {
            if (obj == null) return 1;
            if (obj is ITag otherTagInfo)
                return Position.CompareTo(otherTagInfo.Position);
            throw new ArgumentException("Object is not a ITag");
        }

        public string GetHtmlTag() => $"<picture>{Content}</picture>";
    }
}