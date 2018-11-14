using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.ParserClasses.Nodes;

namespace Markdown.HTMLGeneratorClasses.Visitors.TextVisitors
{
    class PlainTextVisitor
    {
        public string Visit(Node node)
        {
            return $"{node.Value}";
        }
    }
}
