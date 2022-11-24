using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TagUnnumberedListItem : ITag
    {
        public Tag Tag { get; set; }
        public TagInfo TagInfo => new TagInfo(" *", "<li>", "</li>");
        public bool ShouldClose => false;
        public void SetIsSpaceBetween()
        {
            Tag.IsSpaceBetween = true;
        }

        public TagUnnumberedListItem()
        {
            Tag = new Tag(TagType.UnnumberedLisItem, TagInfo, TagStatus.NoOpen);
        }

        public bool IsBegin(char nextCh)
        {
            return true;
        }

        public bool IsEnd(char prevCh)
        {
            return false;
        }
        public Tag GetClosedTagItem()
        {
            var tag = new Tag(TagType.Header, TagInfo, TagStatus.Close);
            Tag.ClosedTag = tag;
            Tag = tag;
            return tag;
        }

        public Tag GetOpenTagItem()
        {
            var tag = new Tag(TagType.UnnumberedLisItem, TagInfo, TagStatus.Open);
            Tag = tag;
            return tag;
        }

        public List<Tag> GetClosedTag(char nextCh = ' ')
        {
            return new List<Tag> { GetClosedTagItem() };
        }

        public List<Tag> GetOpenTag(char prevCh = ' ')
        {
            return new List<Tag> { GetOpenTagItem() };
        }
    }

}
