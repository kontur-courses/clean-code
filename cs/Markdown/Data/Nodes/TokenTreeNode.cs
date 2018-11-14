using System.Collections.Generic;

namespace Markdown.Data.Nodes
{
    public abstract class TokenTreeNode
    {
        public readonly List<TokenTreeNode> Children = new List<TokenTreeNode>();
    }
}