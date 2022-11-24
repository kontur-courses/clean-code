using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TagOneUnderscore : ITag
    {
        public Tag Tag { get; set; }
        public TagInfo TagInfo => new TagInfo("_", "<em>", "</em>");
        public bool ShouldClose => true;

        public void SetIsSpaceBetween()
        {
            Tag.IsSpaceBetween = true;
        }

        public TagOneUnderscore()
        {
            Tag = new Tag(TagType.OneUnderscore, TagInfo, TagStatus.NoOpen);
        }

        public bool IsBegin(char nextCh)
        {
            return (!char.IsDigit(nextCh) && !char.IsWhiteSpace(nextCh) && nextCh != '_');
        }

        public bool IsEnd(char prevCh)
        {
            return (!char.IsWhiteSpace(prevCh) && prevCh != '_');
        }

        public List<Tag> GetClosedTag(char nextCh)
        {
            if (char.IsLetter(nextCh))
                Tag.IsLetterAfter = true;
            if (!Tag.IsLetterAfter && !Tag.IsLetterBefore || !Tag.IsSpaceBetween)
            {
                var tag = new Tag(Tag.TagType, TagInfo, TagStatus.Close);
                Tag.ClosedTag = tag;
                Tag = tag;
                return new List<Tag> { tag };
            }
            else
                Tag.Status = TagStatus.NoOpen;

            return new List<Tag> { new Tag(Tag.TagType, TagInfo, TagStatus.NoOpen) };
        }

        public List<Tag> GetOpenTag(char prevCh)
        {
            var tag = new Tag(TagType.OneUnderscore, TagInfo, TagStatus.Open);
            Tag = tag;
            if (char.IsLetter(prevCh))
                Tag.IsLetterBefore = true;
            return new List<Tag> { tag };
        }

    }

}