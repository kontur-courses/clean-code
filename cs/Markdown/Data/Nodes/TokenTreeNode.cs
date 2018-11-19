using System.Collections.Generic;
using System.Text;
using Markdown.TreeTranslator.NodeTranslator;

namespace Markdown.Data.Nodes
{
    public abstract class TokenTreeNode
    {
        public readonly List<TokenTreeNode> Children = new List<TokenTreeNode>();

        public abstract void Translate(INodeTranslator translator, StringBuilder textBuilder);
    }
}