using Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Entity
{
    public class Paragraph
    {
        public ParagraphType Type { get; private set; }
        public INode Tree { get; private set; }

        public Paragraph(ParagraphType type)
        {
            Type = type;
        }

        public void AddTag(INode tag)
        {

        }
    }
}
