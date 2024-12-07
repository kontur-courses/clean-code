using MarkDownConverter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Nodes
{
    public abstract class Node
    {
        public abstract NodeState State { get; }


        public override bool Equals(object? obj)
        {
            if (this is INodeWithChildren node && obj is INodeWithChildren other)
                return this.GetType() == other.GetType() && node.Children.SequenceEqual(other.Children);
            if (this is SymbolsNode valueNode && obj is SymbolsNode otherValueNode)
                return valueNode.Value.Equals(otherValueNode.Value);
            return false;
        }
    }
}
