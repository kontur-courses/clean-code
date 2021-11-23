using System.Collections.Generic;

namespace Markdown
{
    /// <summary>
    /// Простой кончный автомат состояния которого - поддеревья суффикснового дерева некоторых ключевых слов
    /// </summary>
    public class SimpleTrieStateMachine
    {
        public Trie KeyWordsTrie { get; } = new();

        public SimpleTrieStateMachine(IEnumerable<string> keyWords)
        {
            KeyWordsTrie.Add(keyWords);
        }

        public bool CanUpdateStates(char letter, Trie currentState)
        {
            return currentState.Children.ContainsKey(letter) ||
                   KeyWordsTrie.Children.ContainsKey(letter);
        }

        public bool CanUpdateStates(char letter)
        {
            return KeyWordsTrie.Children.ContainsKey(letter);
        }

        public Trie UpdateStates(char letter, Trie currentState)
        {
            if (currentState.Children.ContainsKey(letter))
                return currentState.Children[letter];

            return currentState.Root.Children.ContainsKey(letter)
                ? currentState.Root.Children[letter]
                : currentState.Root;
        }
    }
}