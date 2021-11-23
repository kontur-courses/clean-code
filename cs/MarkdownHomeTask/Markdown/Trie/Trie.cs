using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Trie
    {
        public readonly string Value;
        private readonly Dictionary<char, Trie> children = new();
        public readonly Trie Root;

        public Trie()
        {
            Value = "";
            IsTerminate = false;
            Root = this;
        }

        private Trie(string value, bool isTerminate, Trie parent)
        {
            Value = value;
            IsTerminate = isTerminate;
            Root = parent.Root;
        }

        public bool IsTerminate { get; private set; }
        public IReadOnlyDictionary<char, Trie> Children => children;

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
            if (!children.ContainsKey(word[position]))
            {
                children[word[position]] = new Trie(value, false, this);
            }

            children[word[position]].Add(word, position + 1);
        }
    }
}