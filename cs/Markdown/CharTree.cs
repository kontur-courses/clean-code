using System.Collections.Generic;

namespace Markdown
{
    public class CharTree
    {
        private readonly CharTreeNode root = new CharTreeNode();

        public void Add(string value)
        {
            var node = root;
            foreach (var item in value)
                node = node.Add(item);
        }

        public int SearchDepth(string value, out int children)
        {
            var node = root;
            int index;
            for (index = 0; index < value.Length; ++index)
            {
                if (node == null)
                    break;
                node = node[value[index]];
            }

            children = node?.CountChildren ?? -1;
            return index - (node == null ? 1 : 0);
        }

        private class CharTreeNode
        {
            private readonly Dictionary<char, CharTreeNode> nodes = new Dictionary<char, CharTreeNode>();

            public int CountChildren => nodes.Count;

            public CharTreeNode this[char value] => nodes.ContainsKey(value) ? nodes[value] : null;

            public CharTreeNode Add(char value)
            {
                if (!nodes.ContainsKey(value))
                    nodes[value] = new CharTreeNode();
                return nodes[value];
            }
        }
    }
}
