using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tag_Classes
{
    public class TagEvent
    {
        public readonly TagSide tagSide;
        public readonly TagKind tagKind;
        public readonly string tagContent;

        public TagEvent(TagSide tagSide, TagKind tagKind, string content)
        {
            this.tagSide = tagSide;
            this.tagKind = tagKind;
            this.tagContent = content;
        }
    }
}
