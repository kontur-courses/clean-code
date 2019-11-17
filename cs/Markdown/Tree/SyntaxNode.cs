using System.Collections.Generic;

namespace Markdown.Tree
{
    public abstract class SyntaxNode
    {
        public List<SyntaxNode> ChildNode { get; }

        protected SyntaxNode(List<SyntaxNode> childNode)
        {
            ChildNode = childNode;
        }

        public abstract string ConvertTo(Dictionary<TagType, Tag> tags);
    }
}