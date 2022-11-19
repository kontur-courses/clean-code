using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public class Tag : ITag
    {
        public TagType Type { get; }
        public string Opening { get; }
        public string Closing { get; }

        public Tag(TagType type, string opening, string closing)
        {
            Type = type;
            Opening = opening;
            Closing = closing;
        }
    }
}
