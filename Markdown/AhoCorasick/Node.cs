using System.Collections;
using System.Collections.Generic;

namespace AhoCorasick
{
    internal class Node<TValue> : IEnumerable<Node<TValue>>
    {
        private readonly Dictionary<char, Node<TValue>> children = new();
        
        public char Word { get; }
        public Node<TValue> Parent { get; }
        public Node<TValue> Suffix { get; set; }
        internal List<TValue> Values { get; } = new();

        internal Node() { }

        public Node(char word, Node<TValue> parent)
        {
            Word = word;
            Parent = parent;
        }
        
        internal Node<TValue> this[char symbol]
        {
            get => children.TryGetValue(symbol, out var result) ? result : null;
            set => children[symbol] = value;
        }

        public IEnumerator<Node<TValue>> GetEnumerator()
        {
            return children.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}