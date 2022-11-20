    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tags
{
    public class Token : ITag
    {
        public TagType Type { get; }
        public string OpeningSubTag { get; }
        public string ClosingSubTag { get; }

        public Token(TagType type, string opening, string closing)
        {
            Type = type;
            OpeningSubTag = opening;
            ClosingSubTag = closing;
        }

        public string GetSubTag(SubTagOrder order)
        {
            if (order == SubTagOrder.Opening)
                return OpeningSubTag;

            return ClosingSubTag;
        }
    }
}
