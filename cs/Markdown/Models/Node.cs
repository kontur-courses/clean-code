using Markdown.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Models
{
    /// <summary>
    /// Представляет узел абстрактного синтаксического дерева (AST)
    /// </summary>
    public class Node
    {
        public NodeType Type { get; }
        public List<Node> ChildrenNodes { get; }
        public string Value { get; }

        public Node(NodeType type, List<Node> childrenNodes, string value)
        {
            Type = type;
            ChildrenNodes = childrenNodes;
            Value = value;
        }
    }
}
