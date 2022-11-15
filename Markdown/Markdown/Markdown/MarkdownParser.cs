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

    internal static class MarkdownParser
    {
        public static List<Token> GetArrayWithMdTags(string stringWithTags)
        {
            var mdTags = new HashSet<string>() { "# ", "__", "_", "\\" };
            var tokenList= new List<Token>();
            var type = TokenType.defaultToken;
            foreach (var tag in mdTags)
            {
                type = type.GetTokenType(tag);
                var fallIndexes= tokenList.GetBusyIndexes();
                foreach (var indexOfTag in tag.GetIndexInLine(stringWithTags))
                {
                    if(!fallIndexes.Contains(indexOfTag))
                        tokenList.Add(new Token(tag, indexOfTag, tag.Length, type));
                }
            }
            return tokenList;
        }

        private static TokenType GetTokenType(this TokenType type, string tag)
        {
            type = tag switch
            {
                "# " => TokenType.header,
                "__" => TokenType.strong,
                "_" => TokenType.em,
                "\\" => TokenType.displayed,
                _ => type
            };
            return type;
        }

        private static List<int> GetBusyIndexes(this List<Token> tokenList)
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






    }
}
