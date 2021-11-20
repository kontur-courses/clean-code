using System.Collections.Generic;

namespace Markdown
{
    /// <summary>
    /// Простой кончный автомат состояния которого - поддеревья суффикснового деревева некоторых ключевых слов
    /// </summary>
    public class SimpleTrieStateMachine
    {
        private readonly Trie keyWordsTrie = new();
        private Trie currentState;
        private Trie maxTerminalTrie;


        public SimpleTrieStateMachine(IEnumerable<string> tags)
        {
            currentState = keyWordsTrie;
            keyWordsTrie.Add(tags);
        }

        /// <summary>
        /// Обновляем состояние автомата, смотрим, содержат ли потомки текущего дерева префекс
        /// с очередной буквой в конце
        /// </summary>
        /// <param name="letter">очередная буква</param>
        /// <returns>true если удалось спуститься ниже по дереву, false если откатились до корня</returns>
        public bool IsUpdateStates(char letter)
        {
            while (true)
            {
                if (currentState.IsTerminate)
                {
                    maxTerminalTrie = currentState;
                }

                var key = string.Concat(currentState.Value, letter);
                if (currentState.Children.ContainsKey(key))
                {
                    currentState = currentState.Children[key];
                    return true;
                }

                if (currentState == keyWordsTrie)
                {
                    return false;
                }

                currentState = keyWordsTrie;
            }
        }

        /// <summary>
        /// возвращаем максимальное слово и обнуляем прогресс
        /// </summary>
        /// <returns>null если не было найдено слово</returns>
        public string GetMaxFoundWord()
        {
            var result = maxTerminalTrie?.Value;
            maxTerminalTrie = null;
            return result;
        }
    }
}