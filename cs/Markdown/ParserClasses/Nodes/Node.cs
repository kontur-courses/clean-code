using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.ParserClasses.Nodes
{
    public class Node
    {
        public string Type { get; }
        public string Value { get; }
        public int Consumed { get; }

        public Node(string type, string value, int consumed)
        {
            Type = type;
            Value = value;
            Consumed = consumed;
        }
    }
}
