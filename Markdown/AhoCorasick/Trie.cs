using System.Collections.Generic;
using System.Linq;

namespace AhoCorasick
{
    public class Trie<TValue>
    {
        private readonly Node<TValue> root = new();

        public void Add(IEnumerable<char> word, TValue value)
        {
            var node = root;

            foreach (var symbol in word)
            {
                var child = node[symbol] ?? (node[symbol] = new Node<TValue>(symbol, node));
                node = child;
            }

            node.Values.Add(value);
        }
        
        public void Build()
        {
            var queue = new Queue<Node<TValue>>();
            queue.Enqueue(root);

            while (queue.Any())
            {
                var node = queue.Dequeue();
                foreach (var child in node)
                    queue.Enqueue(child);

                if (node == root)
                {
                    root.Suffix = root;
                    continue;
                }

                var suffix = node.Parent.Suffix;

                while (suffix[node.Word] == null && suffix != root)
                    suffix = suffix.Suffix;

                node.Suffix = suffix[node.Word] ?? root;
                if (node.Suffix == node) 
                    node.Suffix = root;
            }
        }
        
        public IEnumerable<(TValue, int)> Find(IEnumerable<char> text)
        {
            var node = root;
            var index = 0;

            foreach (var c in text)
            {
                while (node[c] == null && node != root)
                    node = node.Suffix;

                node = node[c] ?? root;

                for (var t = node; t != root; t = t.Suffix)
                {
                    foreach (var value in t.Values)
                    {
                        yield return (value, index - (value.ToString()?.Length - 1) ?? 0);
                    }
                }
                
                index++;
            }
        }
    }
}