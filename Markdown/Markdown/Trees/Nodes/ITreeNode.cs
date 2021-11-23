using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.Trees.Nodes
{
    public interface ITreeNode<out T>
        where T : IToken
    {
        public T Value { get; }
        public ITreeNode<T> Parent { get; }
        public IEnumerable<ITreeNode<T>> NestedNodes { get; }
    }
}