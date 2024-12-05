using System.Collections;

namespace Markdown;

public class BinaryTree<T> : IEnumerable<T>
    where T : IComparable
{
    private TreeNode? root;
    
    public void Add(T key)
    {
        var currentSubtree = root;
        if (Equals(root, null))
        {
            root = new TreeNode(key, null);
            return;
        }

        while (true)
            if (currentSubtree != null && key.CompareTo(currentSubtree.Value) >= 0)
            {
                currentSubtree.HeightOfRight++;
                if (currentSubtree.Right == null)
                {
                    currentSubtree.Right = new TreeNode(key, currentSubtree);
                    return;
                }
                currentSubtree = currentSubtree.Right;
            }
            else
            {
                if (currentSubtree == null) continue;
                currentSubtree.HeightOfLeft++;
                if (currentSubtree.Left == null)
                {
                    currentSubtree.Left = new TreeNode(key, currentSubtree);
                    return;
                }

                currentSubtree = currentSubtree.Left;
            }
    }

    public T this[int i]
    {
        get
        {
            if (root != null && (root.HeightOfRight + root.HeightOfLeft < i || i < 0))
                throw new IndexOutOfRangeException();

            var currentSubtree = root;
            var index = 0;

            while (true)
            {
                if (currentSubtree != null && currentSubtree.HeightOfLeft + index == i)
                    return currentSubtree.Value;
                if (currentSubtree != null && currentSubtree.HeightOfLeft + index > i)
                    currentSubtree = currentSubtree.Left;
                else if (currentSubtree != null && currentSubtree.HeightOfLeft < i)
                {
                    index += currentSubtree.HeightOfLeft + 1;
                    currentSubtree = currentSubtree.Right;
                }
            }
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (root == null) yield break;

        foreach (var subtree in root)
            yield return subtree.Value;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public class TreeNode(T value, TreeNode? ancestor) : IEnumerable<TreeNode>
    {
        public readonly T Value = value;
        public int HeightOfLeft { get; set; }
        public int HeightOfRight { get; set; }

        public TreeNode? Left, Right;
        private readonly TreeNode? ancestor = ancestor;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TreeNode> GetEnumerator()
        {
            var treeNode = this;

            while (!Equals(treeNode.Left, null))
                treeNode = treeNode.Left;

            while (true)
            {
                if (treeNode == null) continue;
                yield return treeNode;

                if (treeNode.Right != null)
                {
                    foreach (var tree in treeNode.Right)
                        yield return tree;
                }

                if (treeNode == this) break;

                treeNode = treeNode.ancestor;
            }
        }
    }
}