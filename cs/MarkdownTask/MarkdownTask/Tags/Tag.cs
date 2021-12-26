using MarkdownTask.Styles;

namespace MarkdownTask.Tags
{
    public class Tag
    {
        public Tag(int startsAt, int tagLength, StyleInfo styleInfo)
        {
            StartsAt = startsAt;
            TagLength = tagLength;
            ContentStartsAt = startsAt + styleInfo.TagPrefix.Length;
            ContentLength = tagLength - styleInfo.TagPrefix.Length - styleInfo.TagAffix.Length;
            TagStyleInfo = styleInfo;
        }

        public int StartsAt { get; }
        public int ContentStartsAt { get; }
        public int ContentLength { get; }
        public int TagLength { get; }
        public StyleInfo TagStyleInfo { get; }
    }
}