using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tag_Classes
{
    public class TagEvent
    {
        public readonly TagSide TagSide;
        public readonly TagKind TagKind;
        public readonly string TagContent;

        public TagEvent(TagSide tagSide, TagKind tagKind, string con)
        {
            TagSide = tagSide;
            TagKind = tagKind;
            TagContent = con;
        }
    }
}
