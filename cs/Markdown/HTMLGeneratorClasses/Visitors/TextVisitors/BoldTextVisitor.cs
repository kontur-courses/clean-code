using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.ParserClasses.Nodes;

namespace Markdown.HTMLGeneratorClasses.Visitors.TextVisitors
{
    class BoldTextVisitor
    {
        public string Visit(Node node)
        {
            return $"<strong>{node.Value}</strong>";
        }
    }
}
