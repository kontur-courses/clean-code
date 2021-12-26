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

        public bool Contains(Tag otherTag)
        {
            return StartsAt <= otherTag.StartsAt
                   && StartsAt + TagLength >= otherTag.StartsAt + otherTag.TagLength;
        }

        public bool IntersectsWith(Tag otherTag)
        {
            var isThisOtherSequence = StartsAt > otherTag.StartsAt
                                      && StartsAt < otherTag.StartsAt + otherTag.TagLength
                                      && StartsAt + TagLength > otherTag.StartsAt + otherTag.TagLength;
            var isOtherThisSequence = StartsAt < otherTag.StartsAt
                                      && StartsAt + TagLength > otherTag.StartsAt
                                      && StartsAt + TagLength < otherTag.StartsAt + otherTag.TagLength;

            return isThisOtherSequence || isOtherThisSequence;
        }
    }
}