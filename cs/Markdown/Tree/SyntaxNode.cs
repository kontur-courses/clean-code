using System.Collections.Generic;

namespace Markdown.Tree
{
    public abstract class SyntaxNode
    {
        public readonly List<SyntaxNode> ChildNode;

        protected SyntaxNode(List<SyntaxNode> childNode)
        {
            ChildNode = childNode;
        }

        public abstract string ConvertTo(Dictionary<TagType, Tag> tags);
    }
}