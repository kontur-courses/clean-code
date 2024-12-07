using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDownConverter.Nodes
{
    public interface INodeWithChildren
    {
        public List<Node> Children { get; }
    }
}
