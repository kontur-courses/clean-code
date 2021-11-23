using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Trees.Nodes
{
    public class TreeNode<T> : ITreeNode<T>
        where T : IToken
    {
        public T Value { get; }
        public ITreeNode<T> Parent { get; }
        public IEnumerable<ITreeNode<T>> NestedNodes { get; }

        public TreeNode(T value, ITreeNode<T> parent, IEnumerable<ITreeNode<T>> nestedNodes)
        {
            Value = value;
            Parent = parent;
            NestedNodes = nestedNodes;
        }
    }
}