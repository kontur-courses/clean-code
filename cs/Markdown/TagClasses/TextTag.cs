using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.TagClasses
{
    public class TextTag : ITextTag
    {
        public string Source { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
        public bool IsEscaped { get; set; }
        public TagType Type { get; set; }

        public TextTag(int pos, TagType type, int length, bool isEscaped = false, string src = "", string name = "")
        {
            Position = pos;
            Source = src;
            Name = name;
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

        public string GetHtmlTag() => $@"<p><img src=""{Source}"" alt=""{Name}""></p>";
    }
}