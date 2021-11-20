using System.Collections.Generic;

namespace Markdown
{
    public class SimpleTrieStateMachine
    {
        private readonly Trie tagsTrie = new();
        private Trie currentTrie;
        private Trie maxTerminalTrie;


        public SimpleTrieStateMachine(IEnumerable<string> tags)
        {
            currentTrie = tagsTrie;
            tagsTrie.Add(tags);
        }

        public bool UpdateStates(char letter)
        {
            while (true)
            {
                if (currentTrie.IsTerminate)
                {
                    maxTerminalTrie = currentTrie;
                }

                var key = string.Concat(currentTrie.Value, letter);
                if (currentTrie.Children.ContainsKey(key))
                {
                    currentTrie = currentTrie.Children[key];
                    return true;
                }

                if (currentTrie == tagsTrie)
                {
                    return false;
                }

                currentTrie = tagsTrie;
            }
        }

        public string GetMaxFoundWord()
        {
            var result = maxTerminalTrie?.Value;
            maxTerminalTrie = null;
            return result;
        }
    }
}