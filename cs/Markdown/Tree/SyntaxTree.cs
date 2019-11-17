using System.Collections.Generic;
using System.Text;

namespace Markdown.Tree
{
    public class SyntaxTree:SyntaxNode
    {
        public SyntaxTree(List<SyntaxNode> childNode) : base(childNode) { }
        
        public SyntaxTree() : base(new List<SyntaxNode>()) { }
        
        public void Add(SyntaxNode node)
        {
            ChildNode.Add(node);
        }

        public override string ConvertTo(Dictionary<TagType, Tag> tags)
        {
            var result = new StringBuilder();
            foreach (var child in ChildNode)
            {
                result.Append(child.ConvertTo(tags));
            }
            return result.ToString();
        }
    }
}