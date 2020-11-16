using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    class TagSubstring
    {
        public readonly string Value;
        public readonly int Index;
        public readonly int Length;
        public readonly TagRole Role;
        public readonly Tag Tag;

        public TagSubstring(string value, int index, int length, Tag tag, TagRole role)
        {
            Value = value;
            Index = index;
            Length = length;
            Role = role;
            Tag = tag;
        }
    }
}
