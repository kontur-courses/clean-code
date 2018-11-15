using System.Collections.Generic;

namespace Markdown
{
    internal static class MarkupLanguage
    {
        public static HashSet<string> KeyWords = new HashSet<string>();
        public static Dictionary<char, List<string>> KeywordsByFirstLetter = new Dictionary<char, List<string>>();
        public static string Screening = @"\";

        static MarkupLanguage()
        {
            // todo get Teg children (container)
            KeyWords.Add("__");
            KeyWords.Add("_");

            KeyWords.Add(@"\");

            foreach (var keyWord in KeyWords)
            {
                if (KeywordsByFirstLetter.ContainsKey(keyWord[0]))
                {
                    KeywordsByFirstLetter[keyWord[0]].Add(keyWord);
                }
                else
                {
                    var list = new List<string> {keyWord};
                    KeywordsByFirstLetter.Add(keyWord[0], list);
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
