using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Markdown
{
    public class Tag
    {
        public TagStatus Status { get; set; }
        public Tag OpeningTag { get; set; }
        public Tag ClosedTag { get; set; }
        public readonly TagType TagType;

        public bool HasLetterBefore { get; set; }
        public bool HasSpaceBetween { get; set; }
        public bool HasLetterAfter { get; set; }

        public List<Tag> InnerTag { get; set; }


        public Tag(TagType tagType ,TagStatus status, Tag openingTag)
        {
            TagType = tagType;
            Status = status;
            OpeningTag = openingTag;
            InnerTag = new List<Tag>();
        }
        public Tag(TagType tagType, TagStatus status)
        {
            TagType = tagType;
            Status = status;
            InnerTag = new List<Tag>();
        }

        public Tag(TagType tagType)
        {
            TagType = tagType;
            InnerTag = new List<Tag>();
        }
    }
}
