using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public enum TokenType
    {
        defaultToken = 0,
        strong = 1,
        header = 2,
        em= 3,
        displayed= 4,

    }

    public static class MarkdownParser
    {
        public static List<Token> GetArrayWithMdTags(string stringWithTags)
        {
            var mdTags = new HashSet<string>() { "# ", "__", "_", "\\" };
            var tokenList= new List<Token>();
            foreach (var tag in mdTags)
                AddTokenTag(stringWithTags, tag, tokenList);
            return tokenList;
        }

        private static void AddTokenTag(string stringWithTags, string tag, List<Token> tokenList)
        {
             
            var busyIndexes = tokenList.GetBusyIndexes();
            foreach (var indexOfTag in tag.GetIndexInLine(stringWithTags))
            {
                if (!busyIndexes.Contains(indexOfTag))
                    tokenList.Add(new Token(indexOfTag, tag.Length));
            }
        }

       

        private static List<int> GetBusyIndexes(this IEnumerable<Token> tokenList)
        {
            return tokenList
                .SelectMany(token => Enumerable.Range(token.Position, token.Length))
                .ToList();  
        }

        public static HashSet<int> GetIndexInLine(this string tag, string line)
        {
            var listIndexes= new HashSet<int>();
            for (var i = 0; i < line.Length; i += tag.Length)
            {
                i = line.IndexOf(tag, i);
                if (i == -1)
                    break;
                listIndexes.Add(i);
            }
            return listIndexes;
        }
        public static TextToken GetTextTokenBetweenTagTokens(this string mdText, Token first, Token second)
        {
            var startText = first.Position+first.Length;
            var len = second.Position - startText;
            var token= new TextToken(len, startText);
            if (mdText.Substring(startText,len).Contains(" "))
                token.HaveSpaces=true;
            return token;
        }

    }
}
