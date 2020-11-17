namespace Markdown
{
    public class AttributeToken : IToken
    {
        public int Length { get; }
        public int StartIndex { get; }
        public ITagInfo TagInfo { get; }
        public int EndTagIndex => StartIndex + Length - 1;
        public string Title { get; }
        public string Attribute { get; }

        public AttributeToken(int startIndex, int length, ITagInfo tagInfo, string title, string attribute)
        {
            StartIndex = startIndex;
            Length = length;
            TagInfo = tagInfo;
            Title = title;
            Attribute = attribute;
        }
    }
}