namespace Markdown.Core.Infrastructure
{
    public class TagInfo
    {
        public string MdTag { get; }
        public string TagName { get; }
        public LocationType LocationType { get; }

        public TagInfo(string mdTag, string tagName, LocationType locationType)
        {
            MdTag = mdTag;
            TagName = tagName;
            LocationType = locationType;
        }
    }
}