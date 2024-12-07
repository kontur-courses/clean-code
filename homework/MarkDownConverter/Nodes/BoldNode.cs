using MarkDownConverter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Nodes
{
    public class BoldNode : Node, INodeWithChildren
    {
        public override NodeState State => NodeState.Bold;
        public List<Node> Children { get; } = [];
    }
}
