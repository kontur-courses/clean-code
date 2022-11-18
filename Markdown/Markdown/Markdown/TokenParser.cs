using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static List<IToken> AddText(string MdText)
        {
            var tokenList = MarkdownParser.GetArrayWithMdTags(MdText).OrderBy(tag => tag.Position).ToList();
            var tokens = new List<IToken>();
            var firstToken = MdText
                .GetTextTokenBetweenTagTokens(new Token(0, 0), tokenList[0]);
            if (firstToken.Length > 0)
                tokens.Add(firstToken);
            var count = tokenList.Count;
            var len = 0;
            var nextTextToken = tokenList[0].Position + tokenList[0].Length;
            for (var i = 0; i < count; i++)
            {
                if (i >= 1)
                {
                    var textToken = MdText.GetTextTokenBetweenTagTokens(tokenList[i - 1], tokenList[i]);
                    if (textToken.Length > 0)
                        len += textToken.Length;
                    //tokens.Add(textToken);
                }

                if (TryFillShielding(MdText, tokenList, count, ref i, ref len))
                    continue;

                var token = tokenList[i];
                if (HaveDigit(MdText, token))
                {
                    len += token.Length;
                    continue;
                }
                token.TokensType = GetTokenType(MdText.Substring(token.Position, token.Length));
                nextTextToken = GetNextTextToken(len, tokens, token, nextTextToken);
                tokens.Add(token);
                len = 0;
            }

            return tokens;
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
        private static int GetNextTextToken(int len, List<IToken> tokens, Token token, int start)
        {
            if (len > 0)
                tokens.Add(new TextToken(len, start));
            return token.Position + token.Length;
        }

        private static bool TryFillShielding(string MdText, List<Token> tokenList, int count, ref int i, ref int len)
        {
            if (MdText[tokenList[i].Position] == '\\')
            {
                if (i + 1 < count
                    && tokenList[i + 1].Position == tokenList[i].Position + 1)
                {
                    len += tokenList[i + 1].Length;
                    //tokens.Add(new TextToken(tokenList[i + 1].Length, tokenList[i + 1].Position));
                    i++;
                    return true;
                }

                len += tokenList[i].Length;
                //tokens.Add(new TextToken(tokenList[i].Length, tokenList[i].Position));
            }

            return false;
        }

        private static TokenType GetTokenType(string tag)
        {
            var type = TokenType.defaultToken;
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

    }
}
            
