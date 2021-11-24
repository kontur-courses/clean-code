using System.Collections.Generic;
using System.Linq;

namespace AhoCorasick
{
    public class Trie
    {
        private readonly Node<string> root = new();

        public void Add(IEnumerable<char> word, string value)
        {
            var node = root;

            foreach (var symbol in word)
            {
                node = node[symbol] ??= new Node<string>(symbol, node);
            }

            node.Values.Add(value);
        }

        private static IEnumerable<Node<string>> EnumerateBfs(Node<string> startNode)
        {
            var queue = new Queue<Node<string>>();
            queue.Enqueue(startNode);
            
            while (queue.Any())
            {
                var node = queue.Dequeue();
                yield return node;
                foreach (var child in node)
                    queue.Enqueue(child);
            }
        }
        
        public void Build()
        {
            foreach (var node in EnumerateBfs(root))
            {
                if (node == root)
                {
                    root.Suffix = root;
                    continue;
                }
            
                var suffix = node.Parent.Suffix;
            
                while (suffix[node.Word] is null && suffix != root)
                    suffix = suffix.Suffix;
            
                node.Suffix = suffix[node.Word] ?? root;
                if (node.Suffix == node) 
                    node.Suffix = root;
            }
        }

        private Node<string> GetNextMachineStateBySymbol(Node<string> machineState, char symbol)
        {
            while (machineState[symbol] is null && machineState != root)
                machineState = machineState.Suffix;
            return machineState[symbol] ?? root;
        }

        private IEnumerable<string> GetAllValuesOfState(Node<string> machineState)
        {
            while (machineState != root)
            {
                foreach (var value in machineState.Values)
                    yield return value;
                machineState = machineState.Suffix;
            }
        }

        public IEnumerable<(string, int)> Find(string text)
        {
            var machineState = root;

            for (var symbolPosition = 0; symbolPosition < text.Length; symbolPosition++)
            {
                machineState = GetNextMachineStateBySymbol(machineState, text[symbolPosition]);
                
                foreach (var value in GetAllValuesOfState(machineState))
                {
                    var foundedWordStartPosition = symbolPosition - value.Length + 1;
                    yield return (value, foundedWordStartPosition);
                }
            }
        }
    }
}