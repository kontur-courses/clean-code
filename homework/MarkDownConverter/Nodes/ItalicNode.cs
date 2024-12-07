using MarkDownConverter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Nodes
{
    public class ItalicNode : Node, INodeWithChildren
    {
        public override NodeState State => NodeState.Italic;
        public List<Node> Children { get; } = [];
    }
}
