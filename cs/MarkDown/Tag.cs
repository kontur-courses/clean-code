using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    class Tag
    {
        public Tag Parent;
        public int PositionInsideParent;
        public TagType TagType;
        public string Text;

        public Tag(Tag parent, int positionInsideParent, TagType tagType, string text)
        {
            Parent = parent;
            PositionInsideParent = positionInsideParent;
            TagType = tagType;
            Text = text;
        }
    }
}
