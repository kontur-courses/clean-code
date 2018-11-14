using System.Collections.Generic;

namespace Markdown
{
    static class MarkupLanguage
    {
        public static HashSet<string> KeyWords = new HashSet<string>();
        public static Dictionary<char, List<string>> KeywordsByFirstLetter = new Dictionary<char, List<string>>();

        static MarkupLanguage()
        {
            KeyWords.Add("__");
            KeyWords.Add("_");

            foreach (var s in KeyWords)
            {
                if (KeywordsByFirstLetter.ContainsKey(s[0]))
                {
                    KeywordsByFirstLetter[s[0]].Add(s);
                }
                else
                {
                    var list = new List<string> {s};
                    KeywordsByFirstLetter.Add(s[0], list);
                }
            }
        }

        public static bool IsKeyWords(string word)
        {
            return KeyWords.Contains(word);
        }

        public static List<string> GetKeyWordsOnFirstLetter(char symbol)
        {
            if (!KeywordsByFirstLetter.ContainsKey(symbol)) return new List<string>();
            var res = KeywordsByFirstLetter[symbol];
            res.Sort((a, b) => b.Length - a.Length);
            return res;
        }
    }
}
