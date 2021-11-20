using System.Collections;
using System.Collections.Generic;

namespace AhoCorasick
{
    internal class Node<TValue> : IEnumerable<Node<TValue>>
    {
        private readonly Dictionary<char, Node<TValue>> children = new();
        
        private char Symbol { get; }
        // internal Node<TValue> Fail { get; set; }
        internal List<TValue> Values { get; } = new();

        internal Node() { }

        internal Node(char symbol)
        {
            Symbol = symbol;
        }
        
        internal Node<TValue> this[char symbol]
        {
            get => children.ContainsKey(symbol) ? children[symbol] : null;
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

        public override string ToString()
        {
            return Symbol.ToString();
        }
    }
}