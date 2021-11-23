using Markdown.Tokens;
using Markdown.Trees.Nodes;

namespace Markdown.Trees
{
    /*
     * Не знаю есть ли смысл в этом классе, но все-таки думаю,
     * что дерево нужно как-то хранить, просто таскать корень кажется не очень.
     * Плюсом возможно придется добавлять какие-то дополнительные методы, так что
     * считаю будет лучшим решением оставить.
     */
    public class MarkingTree<T> : IMarkingTree<T>
        where T : IToken
    {
        public ITreeNode<T> Root { get; }

        public MarkingTree(ITreeNode<T> root)
        {
            Root = root;
        }
    }
}