using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Trie
    {
        public readonly string Value;
        private readonly Dictionary<string, Trie> children = new();

        public Trie()
        {
            Value = "";
            IsTerminate = false;
        }

        private Trie(string value, bool isTerminate)
        {
            Value = value;
            IsTerminate = isTerminate;
        }

        public bool IsTerminate { get; private set; }
        public IReadOnlyDictionary<string, Trie> Children => children;

        public void Add(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                Add(word);
            }
        }

        private void Add(string word, int position = 0)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentException();
            }

            if (position >= word.Length)
            {
                IsTerminate = true;
                return;
            }

            var value = word.Substring(0, position + 1);
            if (!children.ContainsKey(value))
            {
                children[value] = new Trie(value, false);
            }

            children[value].Add(word, position + 1);
        }
    }
}