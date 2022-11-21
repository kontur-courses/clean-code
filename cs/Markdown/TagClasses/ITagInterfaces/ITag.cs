namespace Markdown.TagClasses.ITagInterfaces
{
    public class Tag : IComparable
    {
        public int Position { get; set; }

        public int CompareTo(object? obj)
        {
            if (obj == null) return 1;
            TagInfo? otherTagInfo = obj as TagInfo;
            if (otherTagInfo != null)
                return Position.CompareTo(otherTagInfo.Position);
            throw new ArgumentException("Object is not a TagInfo");
        }
    }
}
