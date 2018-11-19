using System;
using System.Collections.Generic;

namespace Markdown
{
    internal static class MarkupLanguage
    {
        private static readonly HashSet<string> KeyWords = new HashSet<string>();
        private static readonly Dictionary<char, List<string>> KeywordsByFirstLetter = new Dictionary<char, List<string>>();
        public static readonly List<Func<LinkedListNode<string>, bool>> LanguageRules = new List<Func<LinkedListNode<string>, bool>>();
        public static string EscapeCharacter { get; } = "\\";

        static MarkupLanguage()
        {
            // todo get Teg children (container)
            KeyWords.Add("__");
            KeyWords.Add("_");

            KeyWords.Add(EscapeCharacter);

            foreach (var keyWord in KeyWords)
            {
                if (KeywordsByFirstLetter.ContainsKey(keyWord[0]))
                    KeywordsByFirstLetter[keyWord[0]].Add(keyWord);
                else
                    KeywordsByFirstLetter.Add(keyWord[0], new List<string> { keyWord });
            }

            bool LowLineForNumbersRule(LinkedListNode<string> currentNode) => 
                !(currentNode.Value.Contains("_") && (int.TryParse(currentNode.Previous?.Value, out _) || int.TryParse(currentNode.Next?.Value, out _)));

            bool ScreeningRule(LinkedListNode<string> currentNode)
            {
                if (currentNode.Previous?.Value != EscapeCharacter) return currentNode.Value != EscapeCharacter;
                // ReSharper disable once AssignNullToNotNullAttribute
                currentNode.List.Remove(currentNode.Previous);
                return false;
            }

            LanguageRules.Add(LowLineForNumbersRule);
            LanguageRules.Add(ScreeningRule);
        }

        public static bool IsKeyWords(string word) => KeyWords.Contains(word);

        public static List<string> GetKeyWordsOnFirstLetter(char letter)
        {
            if (!KeywordsByFirstLetter.ContainsKey(letter)) return new List<string>();
            var keyWords = KeywordsByFirstLetter[letter];
            keyWords.Sort((a, b) => b.Length - a.Length);
            return keyWords;
        }
    }
}
