using Markdown.Enums;

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
        public string? Title { get; set; }

        public Node(NodeType type, List<Node> childrenNodes, string value)
        {
            Type = type;
            ChildrenNodes = childrenNodes;
            Value = value;
        }
    }
}
