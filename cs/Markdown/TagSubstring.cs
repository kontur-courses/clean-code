using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Markdown
{
    class TagSubstring : Substring
    {
        public readonly TagRole Role;
        public readonly Tag Tag;

        public TagSubstring(int index, string value, Tag tag, TagRole role) : base(index, value)
        {
            Tag = tag;
            Role = role;
        }

        public static TagSubstring FromOpening(int index, Tag tag) =>
            new TagSubstring(index, tag.Opening, tag, TagRole.Opening);

        public static TagSubstring FromEnding(int index, Tag tag) =>
            new TagSubstring(index, tag.Ending, tag, TagRole.Ending);
    }
}
