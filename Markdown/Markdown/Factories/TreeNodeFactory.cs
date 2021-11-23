using System.Collections.Generic;
using Markdown.Tokens;
using Markdown.Trees.Nodes;

namespace Markdown.Factories
{
    public class TreeNodeFactory<T> : ITreeNodeFactory<T>
        where T : IToken
    {
        public ITreeNode<T> NewTreeNode(T value, ITreeNode<T> parent, IEnumerable<ITreeNode<T>> nestedNodes)
        {
            return new TreeNode<T>(value, parent, nestedNodes);
        }
    }
}