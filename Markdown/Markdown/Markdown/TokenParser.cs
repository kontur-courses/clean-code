using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Markdown
{
    public static class TokenParser
    {
        private static char[] digits = new []{'0','1','2','3','4','5','6','7','8','9'};
        public static void DoSomething(string md)
        {
            var tokens = AddText(md);
            foreach (var token in tokens)
            {
            }
        }

        public static List<Token> AddText(string mdString)
        {
            var tokenList = MarkdownParser.GetArrayWithMdTags(mdString).OrderBy(tag => tag.Position).ToList();
            var tokens = new List<Token>();
            var firstToken = mdString
                .GetTextTokenBetweenTagTokens(new Token(0, 0), tokenList[0]);
            if (firstToken.Length > 0)
                tokens.Add(firstToken);
            for (var i = 0; i < tokenList.Count; i++)
            {
                if (AddTextTokens(mdString, i, tokenList, tokens)) 
                    continue;
                var token = tokenList[i];
                token.Type = GetTokenType(mdString.Substring(token.Position, token.Length));
                token.Element = token.GetElementInText(mdString);
                tokens.Add(token);
            }

            return tokens.RemoveFields().RemoveDigits(mdString).ToList();
        }

        private static bool AddTextTokens(string mdString, int i, List<Token> tokenList, List<Token> tokens)
        {
            if (i >= 1)
            {
                var textToken = mdString.GetTextTokenBetweenTagTokens(tokenList[i - 1], tokenList[i]);
                if (textToken.Length > 0)
                    tokens.Add(textToken);
            }

            if (mdString[tokenList[i].Position] == '\\')
            {
                tokens.Add(new Token(tokenList[i].Position, tokenList[i].Length, TokenType.Field));
                return true;
            }

            return false;
        }

        public static IEnumerable<Token> CreatePairTokens(this List<Token> tokens)
        {
            Token previous;
            var first = true;
            foreach (var token in tokens.Where(t=>t.Type!=TokenType.Default))
            {
                previous= token;
                if(first)
                {
                    first=false;
                    continue;
                }

                yield break;
            }
        }

        public static IEnumerable<Token> RemoveFields(this List<Token> tokens)
        {
            var field = false;
            var count= tokens.Count;
            for (var i = 0; i < count; i++)
            {
                var token = tokens[i];
                if (field)
                {
                    field=false;
                    token.ToDefault();
                    yield return token;
                    continue;
                }

                if (token.Type != TokenType.Field)
                {
                    yield return token;
                    continue;
                }
                token.ToDefault();
                if (i + 1 < count && tokens[i + 1].Type != TokenType.Default)
                    field = true;
            }
        }

        public static IEnumerable<Token> RemoveDigits(this IEnumerable<Token> tokens, string mdString)
        {
            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Default)
                    yield return token;
                if (!HaveDigit(mdString, token)) continue;
                token.ToDefault();
                yield return token;
            }
        }
        private static bool HaveDigit(string MdString, Token token)
        {
            if (token.Position - 1 >= 0 && digits.Contains(MdString[token.Position - 1]))
                return true;
            if (token.Position + token.Length < MdString.Length &&
                digits.Contains(MdString[token.Position + token.Length]))
                return true;
            return false;
            //return ((token.Position - 1 > 0 && digits.Contains(MdString[token.Position - 1]))
            //        ||(token.Position + token.Length < MdString.Length && 
            //           digits.Contains(MdString[token.Position + token.Length]))) && false;
        }
        private static TokenType GetTokenType(string tag)
        {
            var type = TokenType.Default;
            type = tag switch
            {
                "# " => TokenType.Header,
                "__" => TokenType.Strong,
                "_" => TokenType.Italic,
                "\\" => TokenType.Field,
                _ => type
            };
            return type;
        }

    }
}
            
