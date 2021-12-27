using MarkdownTask.Styles;

namespace MarkdownTask.Tags
{
    public class Tag
    {
        public Tag(int startsAt, int tagLength, TagStyleInfo tagStyleInfo)
        {
            StartsAt = startsAt;
            TagLength = tagLength;
            ContentStartsAt = startsAt + tagStyleInfo.TagPrefix.Length;
            ContentLength = tagLength - tagStyleInfo.TagPrefix.Length - tagStyleInfo.TagAffix.Length;
            TagTagStyleInfo = tagStyleInfo;
        }

        public int StartsAt { get; }
        public int ContentStartsAt { get; }
        public int ContentLength { get; }
        public int TagLength { get; }
        public Tag NextTag { get; private set; }
        public TagStyleInfo TagTagStyleInfo { get; }

        public void SetNextTag(Tag nextTag)
        {
            NextTag = nextTag;
        }

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