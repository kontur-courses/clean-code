using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Domain.Tags
{
    public class MdTag
    {
        public Tag Tag { get; }
        public int Index { get; }

        public MdTag(Tag tag, int index)
        {
            Tag = tag;
            Index = index;
        }
    }
}
