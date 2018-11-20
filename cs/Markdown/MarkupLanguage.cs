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
        public static bool EscapeMode { get; private set; } = false;

        static MarkupLanguage()
        {
            KeyWords.Add("__");
            KeyWords.Add("_");
            KeyWords.Add("(");
            KeyWords.Add(")");
            KeyWords.Add("[");
            KeyWords.Add("]");
            KeyWords.Add(EscapeCharacter);

            InitializeKeywordsByFirstLetter();
            
            LanguageRules.Add(LowLineForNumbersRule);
            LanguageRules.Add(ScreeningRule);
        }

        private static void InitializeKeywordsByFirstLetter()
        {
            foreach (var keyWord in KeyWords)
            {
                if (KeywordsByFirstLetter.ContainsKey(keyWord[0]))
                    KeywordsByFirstLetter[keyWord[0]].Add(keyWord);
                else
                    KeywordsByFirstLetter.Add(keyWord[0], new List<string> { keyWord });
            }
        }

        private static bool ScreeningRule(LinkedListNode<string> currentNode)
        {
            switch (currentNode.Value)
            {
                case "(":
                    EscapeMode = true;
                    return true;
                case ")":
                    EscapeMode = false;
                    return true;
                default:
                    return EscapeModeRule(currentNode) || EscapeCharacterRule(currentNode);
            }
        }

        private static bool EscapeCharacterRule(LinkedListNode<string> currentNode)
        {
            if (currentNode.Previous == null) return true;
            if (currentNode.Previous.Value != EscapeCharacter) return currentNode.Value != EscapeCharacter;
            currentNode.List.Remove(currentNode.Previous);
            return false;
        }

        private static bool EscapeModeRule(LinkedListNode<string> currentNode)
        {
            if (!EscapeMode) return false;
            if (currentNode.Previous == null || currentNode.Next == null) return true;
            currentNode.Value = currentNode.Previous.Value + currentNode.Value;
            currentNode.List.Remove(currentNode.Previous);
            currentNode.Value += currentNode.Next.Value;
            currentNode.List.Remove(currentNode.Next);
            return true;
        }

        private static bool LowLineForNumbersRule(LinkedListNode<string> currentNode) =>
            !(currentNode.Value.Contains("_") && (int.TryParse(currentNode.Previous?.Value, out _) || int.TryParse(currentNode.Next?.Value, out _)));


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
