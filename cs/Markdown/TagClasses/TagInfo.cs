namespace Markdown.TagClasses
{
    public class TagInfo : IComparable
    {
        public int Position { get; set; }
        public string Content { get; set; }
        public bool CanBeStarter { get; set; }
        public bool CanBeEnder { get; set; }
        public bool IsEscaped { get; set; }
        public bool InPair { get; set; }
        public int Length { get; set; }
        public TagType Type { get; set; }

        public TagInfo()
        {
        }


        public TagInfo(int pos, TagType type, int length = 0, bool canBeStarter = false, bool canBeEnder = false,
            bool isEscaped = false, string content = "")
        {
            Position = pos;
            Content = content;
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
                case TagType.Emphasis:
                    Length = (int)TagType.Emphasis;
                    break;
                default:
                    Length = length;
                    break;
            }
        }

        public int CompareTo(object? obj)
        {
            if (obj == null) return 1;
            TagInfo? otherTagInfo = obj as TagInfo;
            if (otherTagInfo != null)
                return Position.CompareTo(otherTagInfo.Position);
            throw new ArgumentException("Object is not a TagInfo");
        }

        public bool InMiddle() => CanBeStarter && CanBeEnder;

        public string GetHtmlTag()
        {
            var result = "";
            switch (Type)
            {
                case TagType.Strong:
                    result = "<strong>";
                    break;
                case TagType.Emphasis:
                    result = "<em>";
                    break;
                case TagType.Picture:
                    result = $"<picture>{Content}</picture>";
                    break;
                    ;
            }

            if (CanBeEnder)
                result = result.Insert(1, "/");
            return result;
        }
    }
}