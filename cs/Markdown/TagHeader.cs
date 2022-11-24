using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TagHeader : ITag
    {
        public Tag Tag { get; set; }
        public TagInfo TagInfo => new TagInfo("# ", "<h1>", "</h1>");

        public TagHeader()
        {
            Tag = new Tag(TagType.Header, TagInfo, TagStatus.NoOpen);
        }
        public bool ShouldClose => false;
        public void SetIsSpaceBetween()
        {
            Tag.IsSpaceBetween = true;
        }

        public bool IsEnd(char prevCh)
        {
            return false;
        }

        public bool IsBegin(char nextCh)
        {
            return true;
        }

        public List<Tag> GetClosedTag(char nextCh = ' ')
        {
            var tag = new Tag(TagType.Header, TagInfo, TagStatus.Close);
            Tag.ClosedTag = tag;
            Tag = tag;
            return new List<Tag> { tag };
        }

        public List<Tag> GetOpenTag(char prevCh)
        {
            var tag = new Tag(TagType.Header, TagInfo, TagStatus.Open);
            Tag = tag;
            return new List<Tag> { tag };
        }
    }
}
