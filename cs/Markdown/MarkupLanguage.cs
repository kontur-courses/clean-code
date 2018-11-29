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
            KeyWords.Add("__");
            KeyWords.Add("_");
            KeyWords.Add("(");
            KeyWords.Add(")");
            KeyWords.Add("[");
            KeyWords.Add("]");
            KeyWords.Add(EscapeCharacter);

            InitializeKeywordsByFirstLetter();
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

        public static bool IsSelectionOfCharacters(string previousToken, string token, string nextToken) =>
            token.Contains("_") && (int.TryParse(previousToken, out _) || int.TryParse(nextToken, out _));

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
