using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;

namespace Markdown
{
    public class Tag
    {
        public TagStatus Status { get; set; }
        public Tag ClosedTag { get; set; }
        public readonly TagType TagType;
        public TagInfo TagInfo { get; set; }

        public bool IsLetterBefore { get; set; }
        public bool IsSpaceBetween { get; set; }
        public bool IsLetterAfter { get; set; }

        public Tag(TagType tagType, TagInfo tagInfo, TagStatus status) 
        {
            TagType = tagType;
            TagInfo = tagInfo;
            Status = status;
        }

        public Tag(TagType tagType, TagInfo tagInfo) : this(tagType, tagInfo, TagStatus.Open)
        {
        }
    }
}

