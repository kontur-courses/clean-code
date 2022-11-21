using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.TagClasses
{
    public class PairedTag : IPairedTag
    {
        public bool CanBeStarter { get; set; }
        public bool CanBeEnder { get; set; }
        public bool InPair { get; set; }
        public int Position { get; set; }
        public int Length { get; set; }
        public bool IsEscaped { get; set; }
        public TagType Type { get; set; }

        public PairedTag()
        {
        }
        public PairedTag(int pos, TagType type, bool canBeStarter = false, bool canBeEnder = false,
            bool isEscaped = false)
        {
            Position = pos;
            Type = type;
            CanBeStarter = canBeStarter;
            CanBeEnder = canBeEnder;
            IsEscaped = isEscaped;
            InPair = false;
            switch (type)
            {
                case TagType.Strong:
                    Length = (int)TagType.Strong;
                    break;
                default:
                    Length = (int)TagType.Emphasis;
                    break;
            }
        }
        public int CompareTo(object? obj)
        {
            if (obj == null) return 1;
            if (obj is ITag otherTagInfo)
                return Position.CompareTo(otherTagInfo.Position);
            throw new ArgumentException("Object is not a ITag");
        }

        public string GetHtmlTag()
        {
            var result = "<strong>";
            if (Type == TagType.Emphasis)
                result = "<em>";
            if (CanBeEnder)
                result = result.Insert(1, "/");
            return result;
        }

        public bool InMiddle() => CanBeStarter && CanBeEnder;
    }
}