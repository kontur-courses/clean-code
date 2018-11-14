using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.ParserClasses.Nodes
{
    public class ContentNode
    {
        public List<ParagraphNode> Paragraphs { get; }
        public int Consumed { get; }

        public ContentNode(List<ParagraphNode> paragraphs, int consumed)
        {
            Paragraphs = paragraphs;
            Consumed = consumed;
        }
    }
}
