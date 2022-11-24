using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TagUnnumberedList : ITag
    {
        public Tag Tag { get; set; }
        public TagInfo TagInfo => new TagInfo(" *", "<ul>", "</ul>");
        public bool ShouldClose => false;

        private TagUnnumberedListItem innerTag= new TagUnnumberedListItem();
        public void SetIsSpaceBetween()
        {
            Tag.IsSpaceBetween = true;
        }

        public TagUnnumberedList()
        { 
            Tag = new Tag(TagType.UnnumberedList, TagInfo, TagStatus.NoOpen);
        }

        public bool IsBegin(char nextCh)
        {
            return true;
        }

        public bool IsEnd(char prevCh)
        {
            return false;
        }
        public List<Tag> GetClosedTag(char nextCh)
        {
            var tags = new List<Tag>();
            tags.Add(innerTag.GetClosedTagItem());
            var tag = new Tag(TagType.UnnumberedList, TagInfo, TagStatus.Close);
            Tag = tag;
            tags.Add(tag);
            return tags;
        }

        public List<Tag> GetOpenTag(char prevCh)
        {
            var tags = new List<Tag>();
            if(Tag.Status != TagStatus.Open)
            {
                var tag = new Tag(TagType.UnnumberedList, TagInfo, TagStatus.Open);
                Tag = tag;
                tags.Add(tag);
            }
            if(innerTag.Tag.Status==TagStatus.Open)
                tags.Add(innerTag.GetClosedTagItem());
            tags.Add(innerTag.GetOpenTagItem());
            return tags;
        }
    }

}
