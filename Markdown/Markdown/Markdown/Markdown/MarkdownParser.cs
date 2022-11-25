using Markdown.Tokens;

namespace Markdown.Markdown
{
    public static class MarkdownParser
    {
        public static Token[] GetArrayWithMdTags(string stringWithTags)
        {
            if (string.IsNullOrEmpty(stringWithTags))
                throw new ArgumentNullException("String for parse token must not be null or empty");
            var mdTags = new HashSet<string> { "# ", "\n", "!", "[", "]", "(", ")", "__", "_", "\\" };
            var mdTokens = new List<Token>();
            foreach (var tag in mdTags)
                AddTokenTag(stringWithTags, tag, mdTokens);
            return mdTokens.OrderBy(x=>x.Position).ToArray();
        }

        private static void AddTokenTag(string stringWithTags, string tag, List<Token> tokenList)
        {
            var busyIndexes = tokenList.GetBusyIndexes();
            foreach (var indexOfTag in tag.GetIndexInLine(stringWithTags)
                         .Where(indexOfTag => !busyIndexes.Contains(indexOfTag)))
            {
                GetToken(stringWithTags, tag, tokenList, indexOfTag);
            }
        }

        private static void GetToken(string stringWithTags, string tag, List<Token> tokenList, int indexOfTag)
        {
            var token = new Token(indexOfTag, tag.Length, GetTokenType(tag));
            token.Element = token.GetElementInText(stringWithTags);
            tokenList.Add(token);
        }

        private static List<int> GetBusyIndexes(this IEnumerable<Token> tokenList)
        {
            return tokenList
                .SelectMany(token => Enumerable.Range(token.Position, token.Length))
                .ToList();
        }

        public static HashSet<int> GetIndexInLine(this string tag, string line)
        {
            var listIndexes = new HashSet<int>();
            for (var i = 0; i < line.Length; i += tag.Length)
            {
                i = line.IndexOf(tag, i);
                if (i == -1)
                    break;
                listIndexes.Add(i);
            }

            return listIndexes;
        }

        public static Token GetTokenBetween(this Token firstToken, Token secondToken)
        {
            var startText = firstToken.Position + firstToken.Length;
            var len = secondToken.Position - startText;
            var token = new Token(startText, len);
            return token;
        }
        private static TokenType GetTokenType(string tag)
        {
            var type = TokenType.Default;
            type = tag switch
            {
                "# " => TokenType.Header,
                "\n" => TokenType.Header,
                "!" => TokenType.ImageStart,
                "[" => TokenType.ImageDescription,
                "]" => TokenType.ImageDescription,
                "(" => TokenType.Image,
                ")" => TokenType.Image,
                "__" => TokenType.Strong,
                "_" => TokenType.Italic,
                "\\" => TokenType.Field,
                _ => type
            };
            return type;
        }
    }
}
