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
                var child = node[symbol] ?? (node[symbol] = new Node<TValue>(symbol));
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
            }
        }

        public IEnumerable<(TValue, int)> Find(IEnumerable<char> text)
        {
            var node = root;
            var index = 0;

            foreach (var c in text)
            {
                while (node[c] == null && node != root)
                    node = root;

                node = node[c] ?? root;

                if (node != root)
                {
                    var foundedWord = node.Values.FirstOrDefault();
                    yield return (foundedWord, index - (foundedWord?.ToString()?.Length - 1) ?? 0);
                }

                index++;
            }
        }
    }
}