using MarkDownConverter.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Nodes
{
    public class SymbolsNode(string value) : Node
    {
        public override NodeState State => NodeState.Symbols;
        public string Value => value;
    }
}
