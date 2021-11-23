using Markdown.Tokens;
using Markdown.Trees.Nodes;

namespace Markdown.Trees
{
    public interface IMarkingTree<out T>
        where T : IToken
    {
        public ITreeNode<T> Root { get; }
    }
}