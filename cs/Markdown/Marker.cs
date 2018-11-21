using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Marker
    {
        public int Position { get; }
        public Tag Tag { get; }
        public TagType TagType { get; set; }

        public Marker(Tag tag, TagType tagType, int position)
        {
            Tag = tag;
            TagType = tagType;
            Position = position;
        }
    }
}
