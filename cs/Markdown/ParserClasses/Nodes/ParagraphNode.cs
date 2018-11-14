using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.ParserClasses.Nodes
{
    public class ParagraphNode
    {
        public List<Node> Sentences { get; }
        public int Consumed { get; }

        public ParagraphNode(List<Node> sentences, int consumed)
        {
            Sentences = sentences;
            Consumed = consumed;
        }
    }
}
