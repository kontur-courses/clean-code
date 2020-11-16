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

        public TagSubstring(Substring substring, TagRole role, Tag tag) : this(substring.Index, substring.Value, tag, role)
        {}
    }
}
