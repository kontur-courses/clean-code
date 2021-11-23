using Markdown.Tokens;
using Markdown.Trees;
using Markdown.Trees.Nodes;

namespace Markdown.Factories
{
    public class MarkingTreeFactory<T> : IMarkingTreeFactory<T>
        where T : IToken
    {
        public IMarkingTree<T> NewMarkingTree(ITreeNode<T> root)
        {
            return new MarkingTree<T>(root);
        }
    }
}